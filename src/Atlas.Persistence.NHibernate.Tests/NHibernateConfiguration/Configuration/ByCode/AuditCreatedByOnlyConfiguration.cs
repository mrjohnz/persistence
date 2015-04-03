// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedByOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.Testing.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class AuditCreatedByOnlyConfiguration : ClassMapping<AuditCreatedByOnly>
   {
      public AuditCreatedByOnlyConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("AuditCreatedByOnlyID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Guid);

         // TODO: Use convention for this
         this.Property(c => c.CreatedUserGuid, c => { c.Column("CreatedUserGuid"); c.NotNullable(true); });
      }
   }
}
