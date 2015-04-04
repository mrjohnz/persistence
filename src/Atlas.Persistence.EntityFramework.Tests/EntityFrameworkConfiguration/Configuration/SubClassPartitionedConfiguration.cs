// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClassPartitionedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.TestsBase.Entities;

   public class SubClassPartitionedConfiguration : EntityTypeConfiguration<SubClassPartitioned>
   {
      public SubClassPartitionedConfiguration()
      {
         this.ToTable("SubClassPartitioned");
      }
   }
}
