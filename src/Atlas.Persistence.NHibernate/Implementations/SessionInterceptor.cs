// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionInterceptor.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations
{
   using System;

   using Atlas.Core.DateTime;

   using global::NHibernate;
   using global::NHibernate.Proxy;
   using global::NHibernate.Type;

   public class SessionInterceptor : EmptyInterceptor
   {
      private readonly IInterceptUnitOfWork[] interceptors;
      private readonly IAuditConfiguration auditConfiguration;
      private readonly IDateTime dateTime;
      private readonly IUserContext userContext;

      public SessionInterceptor(
         IInterceptUnitOfWork[] interceptors,
         IAuditConfiguration auditConfiguration,
         IDateTime dateTime,
         IUserContext userContext)
      {
         this.interceptors = interceptors;
         this.auditConfiguration = auditConfiguration;
         this.dateTime = dateTime;
         this.userContext = userContext;
      }

      public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
      {
         if (this.auditConfiguration != null)
         {
            var auditDateTime = this.dateTime.DateTime;
            var userGuid = this.userContext.UserGuid;
            var entityType = GetConcreteType(entity);

            this.AuditCreated(entityType, entity, auditDateTime, userGuid, propertyNames, state);
            this.AuditModified(entityType, entity, auditDateTime, userGuid, propertyNames, state);
         }

         foreach (var interceptor in this.interceptors)
         {
            interceptor.Add(new[] { entity });
         }

         return base.OnSave(entity, id, state, propertyNames, types);
      }

      public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
      {
         if (this.auditConfiguration != null)
         {
            var auditDateTime = this.dateTime.DateTime;
            var userGuid = this.userContext.UserGuid;
            var entityType = GetConcreteType(entity);

            this.AuditModified(entityType, entity, auditDateTime, userGuid, propertyNames, currentState);
         }

         foreach (var interceptor in this.interceptors)
         {
            interceptor.Modify(new[] { entity });
         }

         return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
      }

      public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
      {
         foreach (var interceptor in this.interceptors)
         {
            interceptor.Remove(new[] { entity });
         }

         base.OnDelete(entity, id, state, propertyNames, types);
      }

      private static Type GetConcreteType(object entity)
      {
         var proxy = entity as INHibernateProxy;

         if (proxy != null)
         {
            return proxy.HibernateLazyInitializer.PersistentClass;
         }

         if (entity != null)
         {
            return entity.GetType();
         }

         return null;
      }

      private static void Set(string[] propertyNames, object[] propertyValues, string propertyName, object value)
      {
         var index = Array.IndexOf(propertyNames, propertyName);

         if (index != -1)
         {
            propertyValues[index] = value;
         }
      }

      private void SetAuditProperty(Func<IAuditConfiguration, string> getPropertyName, string[] propertyNames, object[] propertyValues, object value)
      {
         var createdDateTimePropertyName = getPropertyName(this.auditConfiguration);

         if (createdDateTimePropertyName != null)
         {
            Set(propertyNames, propertyValues, createdDateTimePropertyName, value);
         }
      }

      private void AuditCreated(Type entityType, object entity, DateTime dateTime, Guid userGuid, string[] propertyNames, object[] propertyValues)
      {
         var entityAsArray = new[] { entity };

         this.auditConfiguration.AuditCreatedDateTime(entityType, entityAsArray, dateTime);
         this.SetAuditProperty(c => c.CreatedDateTimePropertyName(entityType), propertyNames, propertyValues, dateTime);

         this.auditConfiguration.AuditCreatedUserGuid(entityType, entityAsArray, userGuid);
         this.SetAuditProperty(c => c.CreatedUserGuidPropertyName(entityType), propertyNames, propertyValues, userGuid);
      }

      private void AuditModified(Type entityType, object entity, DateTime dateTime, Guid userGuid, string[] propertyNames, object[] propertyValues)
      {
         var entityAsArray = new[] { entity };

         this.auditConfiguration.AuditModifiedDateTime(entityType, entityAsArray, dateTime);
         this.SetAuditProperty(c => c.ModifiedDateTimePropertyName(entityType), propertyNames, propertyValues, dateTime);

         this.auditConfiguration.AuditModifiedUserGuid(entityType, entityAsArray, userGuid);
         this.SetAuditProperty(c => c.ModifiedUserGuidPropertyName(entityType), propertyNames, propertyValues, userGuid);
      }
   }
}
