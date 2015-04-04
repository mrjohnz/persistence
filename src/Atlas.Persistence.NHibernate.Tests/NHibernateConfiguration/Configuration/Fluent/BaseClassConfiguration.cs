// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseClassConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.TestsBase.Entities;

   using FluentNHibernate.Mapping;

   using global::NHibernate.Type;

   public class BaseClassConfiguration : ClassMap<BaseClass>
   {
      public BaseClassConfiguration()
      {
         this.Table("BaseClass");
         this.Id(c => c.ID, "BaseClassID").GeneratedBy.Identity();

         this.Map(c => c.Guid).Not.Nullable();
         this.References(c => c.Foo, "FooID").Cascade.SaveUpdate();
         this.Map(c => c.IntEnum).CustomType<EnumType<IntEnum>>().Not.Nullable();
      }
   }
}
