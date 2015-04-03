// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Implementations
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;

   public class AuditConfiguration : IAuditConfiguration
   {
      private readonly Dictionary<Type, Action<object, DateTime>> auditCreatedAtSetters;
      private readonly Dictionary<Type, Action<object, Guid>> auditCreatedBySetters;
      private readonly Dictionary<Type, Action<object, DateTime>> auditModifiedAtSetters;
      private readonly Dictionary<Type, Action<object, Guid>> auditModifiedBySetters;

      private readonly Dictionary<Type, string> auditCreatedAtPropertyNames;
      private readonly Dictionary<Type, string> auditCreatedByPropertyNames;
      private readonly Dictionary<Type, string> auditModifiedAtPropertyNames;
      private readonly Dictionary<Type, string> auditModifiedByPropertyNames;

      public AuditConfiguration()
      {
         this.auditCreatedAtSetters = new Dictionary<Type, Action<object, DateTime>>();
         this.auditCreatedBySetters = new Dictionary<Type, Action<object, Guid>>();
         this.auditModifiedAtSetters = new Dictionary<Type, Action<object, DateTime>>();
         this.auditModifiedBySetters = new Dictionary<Type, Action<object, Guid>>();

         this.auditCreatedAtPropertyNames = new Dictionary<Type, string>();
         this.auditCreatedByPropertyNames = new Dictionary<Type, string>();
         this.auditModifiedAtPropertyNames = new Dictionary<Type, string>();
         this.auditModifiedByPropertyNames = new Dictionary<Type, string>();
      }

      public IAuditConfiguration Audit<TEntity>(
         Expression<Func<TEntity, DateTime>> createdDateTime,
         Expression<Func<TEntity, Guid>> createdUserGuid,
         Expression<Func<TEntity, DateTime>> modifiedDateTime,
         Expression<Func<TEntity, Guid>> modifiedUserGuid)
      {
         RegisterAuditSetter(createdDateTime, this.auditCreatedAtSetters, this.auditCreatedAtPropertyNames);
         RegisterAuditSetter(createdUserGuid, this.auditCreatedBySetters, this.auditCreatedByPropertyNames);
         RegisterAuditSetter(modifiedDateTime, this.auditModifiedAtSetters, this.auditModifiedAtPropertyNames);
         RegisterAuditSetter(modifiedUserGuid, this.auditModifiedBySetters, this.auditModifiedByPropertyNames);

         return this;
      }

      public IAuditConfiguration AuditCreated<TEntity>(
         Expression<Func<TEntity, DateTime>> createdDateTime,
         Expression<Func<TEntity, Guid>> createdUserGuid)
      {
         RegisterAuditSetter(createdDateTime, this.auditCreatedAtSetters, this.auditCreatedAtPropertyNames);
         RegisterAuditSetter(createdUserGuid, this.auditCreatedBySetters, this.auditCreatedByPropertyNames);

         return this;
      }

      public IAuditConfiguration AuditModified<TEntity>(
         Expression<Func<TEntity, DateTime>> modifiedDateTime,
         Expression<Func<TEntity, Guid>> modifiedUserGuid)
      {
         RegisterAuditSetter(modifiedDateTime, this.auditModifiedAtSetters, this.auditModifiedAtPropertyNames);
         RegisterAuditSetter(modifiedUserGuid, this.auditModifiedBySetters, this.auditModifiedByPropertyNames);

         return this;
      }

      public IAuditConfiguration AuditCreatedDateTime<TEntity>(Expression<Func<TEntity, DateTime>> createdDateTime)
      {
         RegisterAuditSetter(createdDateTime, this.auditCreatedAtSetters, this.auditCreatedAtPropertyNames);

         return this;
      }

      public IAuditConfiguration AuditModifiedDateTime<TEntity>(Expression<Func<TEntity, DateTime>> modifiedDateTime)
      {
         RegisterAuditSetter(modifiedDateTime, this.auditModifiedAtSetters, this.auditModifiedAtPropertyNames);

         return this;
      }

      public IAuditConfiguration AuditCreatedUserGuid<TEntity>(Expression<Func<TEntity, Guid>> createdUserGuid)
      {
         RegisterAuditSetter(createdUserGuid, this.auditCreatedBySetters, this.auditCreatedByPropertyNames);

         return this;
      }

      public IAuditConfiguration AuditModifiedUserGuid<TEntity>(Expression<Func<TEntity, Guid>> modifiedUserGuid)
      {
         RegisterAuditSetter(modifiedUserGuid, this.auditModifiedBySetters, this.auditModifiedByPropertyNames);

         return this;
      }

      public void AuditCreatedDateTime(Type entityType, object[] entities, DateTime dateTime)
      {
         Audit(entityType, this.auditCreatedAtSetters, entities, dateTime);
      }

      public void AuditCreatedUserGuid(Type entityType, object[] entities, Guid userGuid)
      {
         Audit(entityType, this.auditCreatedBySetters, entities, userGuid);
      }

      public void AuditModifiedDateTime(Type entityType, object[] entities, DateTime dateTime)
      {
         Audit(entityType, this.auditModifiedAtSetters, entities, dateTime);
      }

      public void AuditModifiedUserGuid(Type entityType, object[] entities, Guid userGuid)
      {
         Audit(entityType, this.auditModifiedBySetters, entities, userGuid);
      }

      public string CreatedDateTimePropertyName(Type entityType)
      {
         return GetPropertyName(entityType, this.auditCreatedAtPropertyNames);
      }

      public string CreatedUserGuidPropertyName(Type entityType)
      {
         return GetPropertyName(entityType, this.auditCreatedByPropertyNames);
      }

      public string ModifiedDateTimePropertyName(Type entityType)
      {
         return GetPropertyName(entityType, this.auditModifiedAtPropertyNames);
      }

      public string ModifiedUserGuidPropertyName(Type entityType)
      {
         return GetPropertyName(entityType, this.auditModifiedByPropertyNames);
      }

      private static void Audit<TProperty>(Type entityType, IDictionary<Type, Action<object, TProperty>> setters, IEnumerable<object> entities, TProperty value)
      {
         Action<object, TProperty> setter;

         if (!setters.TryGetValue(entityType, out setter))
         {
            return;
         }

         foreach (var entity in entities)
         {
            setter(entity, value);
         }
      }

      private static string GetPropertyName(Type entityType, IDictionary<Type, string> propertyNames)
      {
         string propertyName;

         if (propertyNames.TryGetValue(entityType, out propertyName))
         {
            return propertyName;
         }

         return null;
      }

      private static void RegisterAuditSetter<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> getter, IDictionary<Type, Action<object, TProperty>> setters, IDictionary<Type, string> propertyNames)
      {
         var propertyName = ((MemberExpression)getter.Body).Member.Name;
         propertyNames.Add(typeof(TEntity), propertyName);

         var entityType = typeof(TEntity);
         var p = entityType.GetProperty(propertyName);

         if (p == null)
         {
            throw new InvalidOperationException(string.Format("Property '{0}' of '{1}' not found. Ensure it exists on the public interface.", propertyName, entityType.Name));
         }

         if (!p.CanWrite)
         {
            throw new InvalidOperationException(string.Format("Property '{0}' of '{1}' is not writeable. Ensure it has a setter.", propertyName, entityType.Name));
         }

         var instance = Expression.Parameter(typeof(object), "c");
         var typedInstance = Expression.Variable(entityType, "t");
         var property = Expression.Property(typedInstance, p);
         var value = Expression.Parameter(typeof(TProperty), "value");
         var typeAssignment = Expression.Assign(typedInstance, Expression.Convert(instance, entityType));
         var valueAssignment = Expression.Assign(property, value);

         var block = Expression.Block(
               new[] { typedInstance },
               typeAssignment,
               valueAssignment);

         var setter = Expression.Lambda<Action<object, TProperty>>(block, instance, value).Compile();
         setters.Add(typeof(TEntity), setter);
      }
   }
}
