// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedByOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.TestsBase.Entities;

   public class AuditModifiedByOnlyConfiguration : EntityTypeConfiguration<AuditModifiedByOnly>
   {
      public AuditModifiedByOnlyConfiguration()
      {
         this.ToTable("AuditModifiedByOnly");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("AuditModifiedByOnlyID");
      }
   }
}
