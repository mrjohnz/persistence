// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class AuditCreatedConfiguration : EntityTypeConfiguration<AuditCreated>
   {
      public AuditCreatedConfiguration()
      {
         this.ToTable("AuditCreated");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("AuditCreatedID");

         this.Property(c => c.CreatedDateTime).HasColumnType("datetime2");
      }
   }
}
