// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptimisticConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.TestsBase.Entities;

   public class OptimisticConfiguration : EntityTypeConfiguration<Optimistic>
   {
      public OptimisticConfiguration()
      {
         this.ToTable("Optimistic");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("OptimisticID");

         this.Property(c => c.Version).IsRowVersion();

         this.Property(c => c.DateTimeValue).HasColumnType("datetime2");
      }
   }
}
