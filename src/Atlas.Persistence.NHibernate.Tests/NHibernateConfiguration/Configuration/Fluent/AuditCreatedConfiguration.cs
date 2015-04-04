// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.TestsBase.Entities;

   using FluentNHibernate.Mapping;

   public class AuditCreatedConfiguration : ClassMap<AuditCreated>
   {
      public AuditCreatedConfiguration()
      {
         this.Table("AuditCreated");
         this.Id(c => c.ID).Column("AuditCreatedID").GeneratedBy.Identity();

         this.Map(c => c.Guid);

         // TODO: Use auto-mapper for this
         this.Map(c => c.CreatedDateTime).Not.Nullable();
         this.Map(c => c.CreatedUserGuid).Not.Nullable();
      }
   }
}
