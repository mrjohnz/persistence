// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkAuditInterceptor.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Implementations
{
   using System.Data.Entity.Core.Objects;
   using System.Linq;

   using Atlas.Core.DateTime;

   public class EntityFrameworkAuditInterceptor : IInterceptUnitOfWork
   {
      private readonly IAuditConfiguration auditConfiguration;
      private readonly IDateTime dateTime;
      private readonly IUserContext userContext;

      public EntityFrameworkAuditInterceptor(
         IAuditConfiguration auditConfiguration,
         IDateTime dateTime,
         IUserContext userContext)
      {
         this.auditConfiguration = auditConfiguration;
         this.dateTime = dateTime;
         this.userContext = userContext;
      }

      public void Add(object[] entities)
      {
         var auditDateTime = this.dateTime.DateTime;
         var userGuid = this.userContext.UserGuid;

         foreach (var entitiesByType in entities.GroupBy(c => c.GetType()))
         {
            var entityType = ObjectContext.GetObjectType(entitiesByType.Key);
            var auditEntities = entitiesByType.ToArray();

            this.auditConfiguration.AuditCreatedDateTime(entityType, auditEntities, auditDateTime);
            this.auditConfiguration.AuditCreatedUserGuid(entityType, auditEntities, userGuid);
            this.auditConfiguration.AuditModifiedDateTime(entityType, auditEntities, auditDateTime);
            this.auditConfiguration.AuditModifiedUserGuid(entityType, auditEntities, userGuid);
         }
      }

      public void Modify(object[] entities)
      {
         var auditDateTime = this.dateTime.DateTime;
         var userGuid = this.userContext.UserGuid;

         foreach (var entitiesByType in entities.GroupBy(c => c.GetType()))
         {
            var entityType = ObjectContext.GetObjectType(entitiesByType.Key);
            var auditEntities = entitiesByType.ToArray();

            this.auditConfiguration.AuditModifiedDateTime(entityType, auditEntities, auditDateTime);
            this.auditConfiguration.AuditModifiedUserGuid(entityType, auditEntities, userGuid);
         }
      }

      public void Remove(object[] entities)
      {
         // Nothing to do
      }
   }
}
