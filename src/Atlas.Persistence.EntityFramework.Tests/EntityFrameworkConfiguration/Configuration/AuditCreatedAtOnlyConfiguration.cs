// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedAtOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class AuditCreatedAtOnlyConfiguration : EntityTypeConfiguration<AuditCreatedAtOnly>
   {
      public AuditCreatedAtOnlyConfiguration()
      {
         this.ToTable("AuditCreatedAtOnly");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("AuditCreatedAtOnlyID");

         this.Property(c => c.CreatedDateTime).HasColumnType("datetime2");
      }
   }
}
