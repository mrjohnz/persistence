// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClassPartitionedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class SubClassPartitionedConfiguration : SubclassMap<SubClassPartitioned>
   {
      public SubClassPartitionedConfiguration()
      {
         this.Table("SubClassPartitioned");
         this.KeyColumn("BaseClassID");

         this.Map(c => c.Name).Length(50);
      }
   }
}
