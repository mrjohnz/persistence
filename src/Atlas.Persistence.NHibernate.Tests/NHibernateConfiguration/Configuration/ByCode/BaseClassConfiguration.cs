// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseClassConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.Testing.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;
   using global::NHibernate.Type;

   public class BaseClassConfiguration : ClassMapping<BaseClass>
   {
      public BaseClassConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("BaseClassID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Guid, c => c.NotNullable(true));
         this.ManyToOne(c => c.Foo, c => { c.Column("FooID"); c.Cascade(Cascade.Persist); });
         this.Property(c => c.IntEnum, c => { c.Type<EnumType<IntEnum>>(); c.NotNullable(true); });
      }
   }
}
