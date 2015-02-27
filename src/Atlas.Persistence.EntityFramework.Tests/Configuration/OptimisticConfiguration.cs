namespace Atlas.Domain.Persistence.EntityFramework.Tests
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class OptimisticConfiguration : EntityTypeConfiguration<Optimistic>
   {
      public OptimisticConfiguration()
      {
         this.ToTable("Optimistic");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("OptimisticID");

         this.Property(c => c.StringValue).HasMaxLength(50);
         this.Property(c => c.Version).IsRowVersion();

         this.Property(c => c.DateTimeValue).HasColumnType("datetime2");
      }
   }
}
