// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedByOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class AuditModifiedByOnlyConfiguration : ClassMapping<AuditModifiedByOnly>
   {
      public AuditModifiedByOnlyConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("AuditModifiedByOnlyID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Guid);

         // TODO: Use convention for this
         this.Property(c => c.ModifiedUserGuid, c => { c.Column("ModifiedUserGuid"); c.NotNullable(true); });
      }
   }
}
