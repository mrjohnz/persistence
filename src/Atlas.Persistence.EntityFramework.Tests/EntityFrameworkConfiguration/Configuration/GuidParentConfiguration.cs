// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidParentConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class GuidParentConfiguration : EntityTypeConfiguration<GuidParent>
   {
      public GuidParentConfiguration()
      {
         this.ToTable("GuidParent");
         this.HasKey(c => c.Guid);
         this.Property(c => c.Guid).HasColumnName("GuidParentID");

         this.Property(c => c.CreatedDateTime).HasColumnType("datetime2");
      }
   }
}
