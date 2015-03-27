// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptimisticConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class OptimisticConfiguration : ClassMap<Optimistic>
   {
      public OptimisticConfiguration()
      {
         this.Table("Optimistic");
         this.Id(c => c.ID, "OptimisticID").GeneratedBy.Identity();

         this.Version(c => c.Version);

         this.Map(c => c.StringValue).Length(50);
         this.Map(c => c.IntValue);
         this.Map(c => c.DateTimeValue);
      }
   }
}
