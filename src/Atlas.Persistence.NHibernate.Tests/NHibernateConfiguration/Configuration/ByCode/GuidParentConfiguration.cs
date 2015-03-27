// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidParentConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.Testing.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class GuidParentConfiguration : ClassMapping<GuidParent>
   {
      public GuidParentConfiguration()
      {
         this.Id(c => c.Guid, c => { c.Column("GuidParentID"); c.Generator(Generators.GuidComb); });

         this.Property(c => c.Name, c => c.Length(50));

         // TODO: Use convention for this
         this.Property(c => c.CreatedDateTime, c => { c.Column("CreatedDateTime"); c.NotNullable(true); });
      }
   }
}
