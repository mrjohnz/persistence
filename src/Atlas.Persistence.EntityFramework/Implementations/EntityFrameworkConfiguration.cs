// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Implementations
{
   using System;
   using System.Collections.Generic;
   using System.Configuration;
   using System.Data.Common;
   using System.Data.Entity;
   using System.Data.Entity.Core.Objects;
   using System.Data.Entity.Infrastructure;
   using System.Data.Entity.ModelConfiguration;
   using System.Data.Entity.ModelConfiguration.Conventions;
   using System.Reflection;

   using Atlas.Persistence.EntityFramework;

   public class EntityFrameworkConfiguration : IEntityFrameworkPersistenceConfiguration
   {
      public const string SqlServerProviderName = "System.Data.SqlClient";

      private readonly IList<Type> registeredEntities;
      private readonly DbModelBuilder modelBuilder;

      private string providerName;
      private DbProviderFactory providerFactory;
      private string connectionString;
      private DbCompiledModel compiledModel;

      public EntityFrameworkConfiguration()
      {
         this.registeredEntities = new List<Type>();

         this.modelBuilder = new DbModelBuilder();
         this.modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
         this.modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      }

      // ReSharper disable once ParameterHidesMember
      public IEntityFrameworkPersistenceConfiguration ProviderName(string providerName)
      {
         this.providerName = providerName;
         this.providerFactory = DbProviderFactories.GetFactory(this.providerName);

         return this;
      }

      // ReSharper disable once ParameterHidesMember
      public IEntityFrameworkPersistenceConfiguration ConnectionString(string connectionString)
      {
         this.connectionString = connectionString;

         return this;
      }

      public IEntityFrameworkPersistenceConfiguration ConnectionStringName(string connectionStringName)
      {
         if (this.connectionString != null)
         {
            throw new InvalidOperationException("ConnectionString already set");
         }

         var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

         if (connectionStringSettings == null)
         {
            throw new ArgumentException(string.Format("ConnectionStringName '{0}' not found", connectionStringName));
         }

         if (!string.IsNullOrEmpty(connectionStringSettings.ProviderName))
         {
            if (this.providerName != null && connectionStringSettings.ProviderName != this.providerName)
            {
               throw new InvalidOperationException("ProviderName already set to a different value");
            }

            this.providerName = connectionStringSettings.ProviderName;
         }
         else if (string.IsNullOrEmpty(this.providerName))
         {
            throw new InvalidOperationException("ProviderName not found");
         }

         this.providerFactory = DbProviderFactories.GetFactory(this.providerName);
         this.connectionString = connectionStringSettings.ConnectionString;

         return this;
      }

      public IEntityFrameworkPersistenceConfiguration RegisterEntitiesFromAssemblyOf<T>()
      {
         return this.RegisterEntitiesFromAssembly(typeof(T).Assembly);
      }

      public IEntityFrameworkPersistenceConfiguration RegisterEntitiesFromAssembly(Assembly assembly)
      {
         var entities = new List<Type>();

         foreach (var type in assembly.GetExportedTypes())
         {
            if (!type.IsClass || type.BaseType == null || !type.BaseType.IsGenericType)
            {
               continue;
            }

            var containedType = type.BaseType.GetGenericArguments()[0];

            if (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>))
            {
               var genericMethod = typeof(EntityFrameworkConfiguration).GetMethod("AddEntityType", BindingFlags.Instance | BindingFlags.NonPublic);

               // Create a reflected method from the above method using the correct generic arguments
               var method = genericMethod.GetGenericMethodDefinition().MakeGenericMethod(type, containedType);

               method.Invoke(this, null);

               PrepareRegistration(containedType, entities);
            }
            else if (type.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>))
            {
               var genericMethod = typeof(EntityFrameworkConfiguration).GetMethod("AddComplexType", BindingFlags.Instance | BindingFlags.NonPublic);

               // Create a reflected method from the above method using the correct generic arguments
               var method = genericMethod.GetGenericMethodDefinition().MakeGenericMethod(type, containedType);

               method.Invoke(this, null);
            }
         }

         foreach (var entity in entities)
         {
            this.registeredEntities.Add(entity);
         }

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
         using (var context = this.CreateObjectContext())
         {
            context.CreateDatabase();
         }
      }

      public ObjectContext CreateObjectContext()
      {
         var connection = this.CreateConnection();

         if (this.compiledModel == null)
         {
            this.compiledModel = this.modelBuilder.Build(connection).Compile();
         }

         return this.compiledModel.CreateObjectContext<ObjectContext>(connection);
      }

      private static void PrepareRegistration(Type entity, List<Type> entities)
      {
         var type = entity;

         while (type != null && type != typeof(object))
         {
            // Ignore entity if type or base type is already registered
            if (entities.Contains(type))
            {
               return;
            }

            type = type.BaseType;
         }

         var i = 0;

         while (i < entities.Count)
         {
            var otherEntity = entities[i];

            // Remove any sub-classes of entity
            if (otherEntity.IsSubclassOf(entity))
            {
               entities.RemoveAt(i);
            }
            else
            {
               i++;
            }
         }

         entities.Add(entity);
      }

      private DbConnection CreateConnection()
      {
         if (this.providerFactory == null)
         {
            throw new InvalidOperationException("ProviderFactory cannot be determined");
         }

         if (this.connectionString == null)
         {
            throw new InvalidOperationException("ConnectionString must be specified");
         }

         var connection = this.providerFactory.CreateConnection();

         if (connection == null)
         {
            throw new InvalidOperationException("Failed to create connection");
         }

         connection.ConnectionString = this.connectionString;

         return connection;
      }

      // ReSharper disable UnusedMember.Local
      private void AddEntityType<TEntityConfiguration, TEntityType>()
         // ReSharper restore UnusedMember.Local
         where TEntityConfiguration : EntityTypeConfiguration<TEntityType>, new()
         where TEntityType : class
      {
         var configuration = new TEntityConfiguration();

         this.modelBuilder.Configurations.Add(configuration);
      }

      // ReSharper disable UnusedMember.Local
      private void AddComplexType<TComplexConfiguration, TComplexType>()
         // ReSharper restore UnusedMember.Local
         where TComplexConfiguration : ComplexTypeConfiguration<TComplexType>, new()
         where TComplexType : class
      {
         var configuration = new TComplexConfiguration();

         this.modelBuilder.Configurations.Add(configuration);
      }
   }
}
