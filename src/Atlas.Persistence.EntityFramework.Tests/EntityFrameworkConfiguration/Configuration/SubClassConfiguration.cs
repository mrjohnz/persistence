// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClassConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.TestsBase.Entities;

   public class SubClassConfiguration : EntityTypeConfiguration<SubClass>
   {
      public SubClassConfiguration()
      {
         this.ToTable("SubClass");
      }
   }
}
