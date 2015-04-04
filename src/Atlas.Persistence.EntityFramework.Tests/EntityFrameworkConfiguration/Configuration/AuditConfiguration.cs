// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.TestsBase.Entities;

   public class AuditConfiguration : EntityTypeConfiguration<Audit>
   {
      public AuditConfiguration()
      {
         this.ToTable("Audit");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("AuditID");

         this.Property(c => c.CreatedDateTime).HasColumnType("datetime2");
         this.Property(c => c.ModifiedDateTime).HasColumnType("datetime2");
      }
   }
}
