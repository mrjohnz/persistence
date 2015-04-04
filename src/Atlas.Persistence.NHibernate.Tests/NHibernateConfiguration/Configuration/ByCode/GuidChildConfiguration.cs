// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidChildConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class GuidChildConfiguration : ClassMapping<GuidChild>
   {
      public GuidChildConfiguration()
      {
         this.Id(c => c.Guid, c => { c.Column("GuidChildID"); c.Generator(Generators.GuidComb); });

         this.ManyToOne(c => c.GuidParent, c => { c.Column("GuidParentID"); c.NotNullable(true); c.Cascade(Cascade.Persist); });
      }
   }
}
