// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class AuditConfiguration : ClassMapping<Audit>
   {
      public AuditConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("AuditID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Guid);

         // TODO: Use convention for this
         this.Property(c => c.CreatedDateTime, c => { c.Column("CreatedDateTime"); c.NotNullable(true); });
         this.Property(c => c.CreatedUserGuid, c => { c.Column("CreatedUserGuid"); c.NotNullable(true); });
         this.Property(c => c.ModifiedDateTime, c => { c.Column("ModifiedDateTime"); c.NotNullable(true); });
         this.Property(c => c.ModifiedUserGuid, c => { c.Column("ModifiedUserGuid"); c.NotNullable(true); });
      }
   }
}
