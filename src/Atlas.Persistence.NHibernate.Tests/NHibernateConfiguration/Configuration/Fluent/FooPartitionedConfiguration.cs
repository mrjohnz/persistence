// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FooPartitionedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.TestsBase.Entities;

   using FluentNHibernate.Mapping;

   using global::NHibernate.Type;

   public class FooPartitionedConfiguration : ClassMap<FooPartitioned>
   {
      public FooPartitionedConfiguration()
      {
         this.Table("FooPartitioned");
         this.Id(c => c.ID, "FooID").GeneratedBy.Identity();

         this.Map(c => c.Guid).Not.Nullable();
         this.Map(c => c.PartitionGuid).Not.Nullable();
         this.Map(c => c.IntEnum).CustomType<EnumType<IntEnum>>().Not.Nullable();
         this.Map(c => c.DateTimeValue);
         this.Map(c => c.IntValue);
         this.Map(c => c.StringValue).Length(50);
      }
   }
}
