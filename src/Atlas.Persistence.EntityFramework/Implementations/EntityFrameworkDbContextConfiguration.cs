// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkDbContextConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Implementations
{
   using System;
   using System.Data.Entity;
   using System.Data.Entity.Core.Objects;
   using System.Data.Entity.Infrastructure;

   public class EntityFrameworkDbContextConfiguration<TDbContext> : IEntityFrameworkPersistenceConfiguration
      where TDbContext : DbContext
   {
      private readonly Func<string, TDbContext> contextFactory;

      private string connectionStringOrName;

      public EntityFrameworkDbContextConfiguration(Func<string, TDbContext> contextFactory)
      {
         this.contextFactory = contextFactory;
      }

      // ReSharper disable once ParameterHidesMember
      public IEntityFrameworkPersistenceConfiguration ConnectionString(string connectionString)
      {
         this.connectionStringOrName = connectionString;

         return this;
      }

      public IEntityFrameworkPersistenceConfiguration ConnectionStringName(string connectionStringName)
      {
         this.connectionStringOrName = connectionStringName;

         return this;
      }

      public string[] SchemaCreationScript()
      {
         using (var context = this.CreateObjectContext())
         {
            return context.CreateDatabaseScript().Split(new[] { "\r\n" }, StringSplitOptions.None);
         }
      }

      public void CreateSchema()
      {
         using (var context = this.contextFactory(this.connectionStringOrName))
         {
            context.Database.Create();
         }
      }

      public ObjectContext CreateObjectContext()
      {
         var context = this.contextFactory(this.connectionStringOrName);

         var objectContext = ((IObjectContextAdapter)context).ObjectContext;
         return objectContext;
      }
   }
}
