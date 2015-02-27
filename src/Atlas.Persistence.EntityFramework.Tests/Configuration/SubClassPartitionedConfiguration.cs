namespace Atlas.Domain.Persistence.EntityFramework.Tests
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class SubClassPartitionedConfiguration : EntityTypeConfiguration<SubClassPartitioned>
   {
      public SubClassPartitionedConfiguration()
      {
         this.ToTable("SubClassPartitioned");

         Property(c => c.Name).HasMaxLength(50);
      }
   }
}
