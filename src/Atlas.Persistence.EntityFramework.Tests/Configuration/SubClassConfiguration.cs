namespace Atlas.Domain.Persistence.EntityFramework.Tests
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class SubClassConfiguration : EntityTypeConfiguration<SubClass>
   {
      public SubClassConfiguration()
      {
         ToTable("SubClass");

         Property(c => c.Name).HasMaxLength(50);
      }
   }
}
