// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FooPartitionedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class FooPartitionedConfiguration : EntityTypeConfiguration<FooPartitioned>
   {
      public FooPartitionedConfiguration()
      {
         this.ToTable("FooPartitioned");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("FooID");

         this.Property(c => c.DateTimeValue).HasColumnType("datetime2");
      }
   }
}
