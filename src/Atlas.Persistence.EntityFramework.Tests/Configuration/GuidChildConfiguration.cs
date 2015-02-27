// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidChildConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Domain.Persistence.EntityFramework.Tests
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class GuidChildConfiguration : EntityTypeConfiguration<GuidChild>
   {
      public GuidChildConfiguration()
      {
         this.ToTable("GuidChild");
         this.HasKey(c => c.Guid);
         this.Property(c => c.Guid).HasColumnName("GuidChildID");

         this.HasRequired<GuidParent>(c => c.GuidParent).WithMany().Map(c => c.MapKey("GuidParentID")).WillCascadeOnDelete(false);
      }
   }
}
