// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedAtOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.Testing.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class AuditCreatedAtOnlyConfiguration : ClassMapping<AuditCreatedAtOnly>
   {
      public AuditCreatedAtOnlyConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("AuditCreatedAtOnlyID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Guid);

         // TODO: Use convention for this
         this.Property(c => c.CreatedDateTime, c => { c.Column("CreatedDateTime"); c.NotNullable(true); });
      }
   }
}
