namespace Atlas.Domain.Persistence.EntityFramework.Tests
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class FooConfiguration : EntityTypeConfiguration<Foo>
   {
      public FooConfiguration()
      {
         this.ToTable("Foo");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("FooID");

         this.Property(c => c.DateTimeValue).HasColumnType("datetime2");
         this.Property(c => c.StringValue).HasMaxLength(50);
      }
   }
}
