// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FooConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.TestsBase.Entities;

   public class FooConfiguration : EntityTypeConfiguration<Foo>
   {
      public FooConfiguration()
      {
         this.ToTable("Foo");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("FooID");

         this.Property(c => c.DateTimeValue).HasColumnType("datetime2");
      }
   }
}
