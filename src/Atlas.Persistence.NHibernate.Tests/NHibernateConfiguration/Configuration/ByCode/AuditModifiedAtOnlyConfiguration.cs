// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedAtOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class AuditModifiedAtOnlyConfiguration : ClassMapping<AuditModifiedAtOnly>
   {
      public AuditModifiedAtOnlyConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("AuditModifiedAtOnlyID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Guid);

         // TODO: Use convention for this
         this.Property(c => c.ModifiedDateTime, c => { c.Column("ModifiedDateTime"); c.NotNullable(true); });
      }
   }
}
