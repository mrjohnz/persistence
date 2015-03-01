// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompareContext.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using System.Data.Entity;
   using System.Data.Entity.ModelConfiguration.Conventions;

   using Atlas.Persistence.EntityFramework.Conventions;
   using Atlas.Persistence.Testing.Entities;

   public class CompareContext : DbContext
   {
      public CompareContext(string nameOrConnectionString)
         : base(nameOrConnectionString)
      {
      }

      public DbSet<BaseClass> BaseClasses { get; set; }

      public DbSet<BaseClassPartitioned> BaseClassPartitioned { get; set; }

      public DbSet<Foo> Foos { get; set; }

      public DbSet<FooPartitioned> FooPartitioned { get; set; }

      public DbSet<GuidChild> GuidChildren { get; set; }

      public DbSet<GuidParent> GuidParents { get; set; }

      public DbSet<Optimistic> Optimistics { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
         modelBuilder.Conventions.Add<DateTime2Convention>();
         modelBuilder.Conventions.Add<ForeignKeyNamingConvention>();

         modelBuilder.Entity<SubClass>().ToTable("SubClass");
         modelBuilder.Entity<SubClassPartitioned>().ToTable("SubClassPartitioned");

         modelBuilder.Entity<GuidChild>().HasKey(c => c.Guid);
         modelBuilder.Entity<GuidParent>().HasKey(c => c.Guid);

         modelBuilder.Entity<BaseClass>().Property(c => c.ID).HasColumnName("BaseClassID");
         modelBuilder.Entity<BaseClassPartitioned>().Property(c => c.ID).HasColumnName("BaseClassID");
         modelBuilder.Entity<Foo>().Property(c => c.ID).HasColumnName("FooID");
         modelBuilder.Entity<FooPartitioned>().Property(c => c.ID).HasColumnName("FooID");
         modelBuilder.Entity<GuidChild>().Property(c => c.Guid).HasColumnName("GuidChildID");
         modelBuilder.Entity<GuidParent>().Property(c => c.Guid).HasColumnName("GuidParentID");
         modelBuilder.Entity<Optimistic>().Property(c => c.ID).HasColumnName("OptimisticID");

         modelBuilder.Entity<Optimistic>().Property(c => c.Version).IsRowVersion();
      }
   }
}