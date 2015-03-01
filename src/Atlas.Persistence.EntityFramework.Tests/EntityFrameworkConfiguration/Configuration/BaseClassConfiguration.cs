// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseClassConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration
{
   using System.Data.Entity.ModelConfiguration;

   using Atlas.Persistence.Testing.Entities;

   public class BaseClassConfiguration : EntityTypeConfiguration<BaseClass>
   {
      public BaseClassConfiguration()
      {
         this.ToTable("BaseClass");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("BaseClassID");

         this.HasOptional<Foo>(c => c.Foo).WithMany().Map(c => c.MapKey("FooID")).WillCascadeOnDelete(false);
      }
   }
}
