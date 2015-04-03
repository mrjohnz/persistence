// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuditConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;
   using System.Linq.Expressions;

   public interface IAuditConfiguration
   {
      IAuditConfiguration Audit<TEntity>(
         Expression<Func<TEntity, DateTime>> createdDateTime,
         Expression<Func<TEntity, Guid>> createdUserGuid,
         Expression<Func<TEntity, DateTime>> modifiedDateTime,
         Expression<Func<TEntity, Guid>> modifiedUserGuid);

      IAuditConfiguration AuditCreated<TEntity>(
         Expression<Func<TEntity, DateTime>> createdDateTime,
         Expression<Func<TEntity, Guid>> createdUserGuid);

      IAuditConfiguration AuditModified<TEntity>(
         Expression<Func<TEntity, DateTime>> modifiedDateTime,
         Expression<Func<TEntity, Guid>> modifiedUserGuid);

      IAuditConfiguration AuditCreatedDateTime<TEntity>(Expression<Func<TEntity, DateTime>> createdDateTime);

      IAuditConfiguration AuditModifiedDateTime<TEntity>(Expression<Func<TEntity, DateTime>> modifiedDateTime);

      IAuditConfiguration AuditCreatedUserGuid<TEntity>(Expression<Func<TEntity, Guid>> createdUserGuid);

      IAuditConfiguration AuditModifiedUserGuid<TEntity>(Expression<Func<TEntity, Guid>> modifiedUserGuid);

      void AuditCreatedDateTime(Type entityType, object[] entities, DateTime dateTime);

      void AuditCreatedUserGuid(Type entityType, object[] entities, Guid userGuid);

      void AuditModifiedDateTime(Type entityType, object[] entities, DateTime dateTime);

      void AuditModifiedUserGuid(Type entityType, object[] entities, Guid userGuid);

      string CreatedDateTimePropertyName(Type entityType);

      string CreatedUserGuidPropertyName(Type entityType);

      string ModifiedDateTimePropertyName(Type entityType);

      string ModifiedUserGuidPropertyName(Type entityType);
   }
}
