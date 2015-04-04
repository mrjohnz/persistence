// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class AuditCreatedConfiguration : ClassMapping<AuditCreated>
   {
      public AuditCreatedConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("AuditCreatedID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Guid);

         // TODO: Use convention for this
         this.Property(c => c.CreatedDateTime, c => { c.Column("CreatedDateTime"); c.NotNullable(true); });
         this.Property(c => c.CreatedUserGuid, c => { c.Column("CreatedUserGuid"); c.NotNullable(true); });
      }
   }
}
