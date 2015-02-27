namespace Atlas.Domain.Persistence.EntityFramework.Tests
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
         this.Property(c => c.StringValue).HasMaxLength(50);
      }
   }
}
