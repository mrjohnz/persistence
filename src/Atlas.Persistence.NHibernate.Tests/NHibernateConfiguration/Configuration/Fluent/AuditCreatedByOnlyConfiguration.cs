// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedByOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class AuditCreatedByOnlyConfiguration : ClassMap<AuditCreatedByOnly>
   {
      public AuditCreatedByOnlyConfiguration()
      {
         this.Table("AuditCreatedByOnly");
         this.Id(c => c.ID).Column("AuditCreatedByOnlyID").GeneratedBy.Identity();

         this.Map(c => c.Guid);

         // TODO: Use auto-mapper for this
         this.Map(c => c.CreatedUserGuid).Not.Nullable();
      }
   }
}
