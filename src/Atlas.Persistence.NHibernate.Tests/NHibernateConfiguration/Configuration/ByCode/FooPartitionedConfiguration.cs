// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FooPartitionedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.Testing.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;
   using global::NHibernate.Type;

   public class FooPartitionedConfiguration : ClassMapping<FooPartitioned>
   {
      public FooPartitionedConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("FooID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Guid, c => c.NotNullable(true));
         this.Property(c => c.PartitionGuid, c => c.NotNullable(true));
         this.Property(c => c.IntEnum, c => { c.Type<EnumType<IntEnum>>(); c.NotNullable(true); });
         this.Property(c => c.DateTimeValue);
         this.Property(c => c.IntValue);
         this.Property(c => c.StringValue, c => c.Length(50));
      }
   }
}
