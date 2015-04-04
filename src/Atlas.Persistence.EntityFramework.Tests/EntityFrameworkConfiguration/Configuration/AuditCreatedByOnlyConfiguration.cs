// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedByOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.TestsBase.Entities;

   public class AuditCreatedByOnlyConfiguration : EntityTypeConfiguration<AuditCreatedByOnly>
   {
      public AuditCreatedByOnlyConfiguration()
      {
         this.ToTable("AuditCreatedByOnly");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("AuditCreatedByOnlyID");
      }
   }
}
