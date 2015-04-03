// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedAtOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class AuditCreatedAtOnlyConfiguration : ClassMap<AuditCreatedAtOnly>
   {
      public AuditCreatedAtOnlyConfiguration()
      {
         this.Table("AuditCreatedAtOnly");
         this.Id(c => c.ID).Column("AuditCreatedAtOnlyID").GeneratedBy.Identity();

         this.Map(c => c.Guid);

         // TODO: Use auto-mapper for this
         this.Map(c => c.CreatedDateTime).Not.Nullable();
      }
   }
}
