// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkAuditInterceptor.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Implementations
{
   using System.Data.Entity.Core.Objects;
   using System.Linq;

   public class EntityFrameworkAuditInterceptor : IInterceptUnitOfWork
   {
      private readonly IAuditConfiguration auditConfiguration;
      private readonly IDateTimeFacility dateTimeFacility;
      private readonly IUserContext userContext;

      public EntityFrameworkAuditInterceptor(
         IAuditConfiguration auditConfiguration,
         IDateTimeFacility dateTimeFacility,
         IUserContext userContext)
      {
         this.auditConfiguration = auditConfiguration;
         this.dateTimeFacility = dateTimeFacility;
         this.userContext = userContext;
      }

      public void Add(object[] entities)
      {
         var dateTime = this.dateTimeFacility.CurrentTime;
         var userGuid = this.userContext.UserGuid;

         foreach (var entitiesByType in entities.GroupBy(c => c.GetType()))
         {
            var entityType = ObjectContext.GetObjectType(entitiesByType.Key);
            var auditEntities = entitiesByType.ToArray();

            this.auditConfiguration.AuditCreatedDateTime(entityType, auditEntities, dateTime);
            this.auditConfiguration.AuditCreatedUserGuid(entityType, auditEntities, userGuid);
            this.auditConfiguration.AuditModifiedDateTime(entityType, auditEntities, dateTime);
            this.auditConfiguration.AuditModifiedUserGuid(entityType, auditEntities, userGuid);
         }
      }

      public void Modify(object[] entities)
      {
         var dateTime = this.dateTimeFacility.CurrentTime;
         var userGuid = this.userContext.UserGuid;

         foreach (var entitiesByType in entities.GroupBy(c => c.GetType()))
         {
            var entityType = ObjectContext.GetObjectType(entitiesByType.Key);
            var auditEntities = entitiesByType.ToArray();

            this.auditConfiguration.AuditModifiedDateTime(entityType, auditEntities, dateTime);
            this.auditConfiguration.AuditModifiedUserGuid(entityType, auditEntities, userGuid);
         }
      }

      public void Remove(object[] entities)
      {
         // Nothing to do
      }
   }
}
