// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.TestsBase.Entities;

   using FluentNHibernate.Mapping;

   public class BarConfiguration : ClassMap<Bar>
   {
      public BarConfiguration()
      {
         this.Table("Bar");
         this.Id(c => c.ID, "BarID").GeneratedBy.Identity();

         this.Map(c => c.Name, "Name").Length(50).Not.Nullable().UniqueKey("UK_Site_Name");
      }
   }
}
