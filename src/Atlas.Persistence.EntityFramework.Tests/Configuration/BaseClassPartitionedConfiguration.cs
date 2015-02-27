﻿namespace Atlas.Domain.Persistence.EntityFramework.Tests
{
   using System.Data.Entity.ModelConfiguration;
   using Atlas.Persistence.Testing.Entities;

   public class BaseClassPartitionedConfiguration : EntityTypeConfiguration<BaseClassPartitioned>
   {
      public BaseClassPartitionedConfiguration()
      {
         this.ToTable("BaseClassPartitioned");
         this.HasKey(c => c.ID);
         this.Property(c => c.ID).HasColumnName("BaseClassID");

         this.HasOptional<FooPartitioned>(c => c.Foo).WithMany().Map(c => c.MapKey("FooID")).WillCascadeOnDelete(false);
      }
   }
}
