// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedAtOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.TestsBase.Entities;

   public class AuditModifiedAtOnlyConfiguration : EntityTypeConfiguration<AuditModifiedAtOnly>
   {
      public AuditModifiedAtOnlyConfiguration()
      {
         this.ToTable("AuditModifiedAtOnly");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("AuditModifiedAtOnlyID");

         this.Property(c => c.ModifiedDateTime).HasColumnType("datetime2");
      }
   }
}
