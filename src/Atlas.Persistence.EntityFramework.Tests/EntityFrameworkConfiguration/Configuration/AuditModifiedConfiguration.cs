// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class AuditModifiedConfiguration : EntityTypeConfiguration<AuditModified>
   {
      public AuditModifiedConfiguration()
      {
         this.ToTable("AuditModified");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("AuditModifiedID");

         this.Property(c => c.ModifiedDateTime).HasColumnType("datetime2");
      }
   }
}
