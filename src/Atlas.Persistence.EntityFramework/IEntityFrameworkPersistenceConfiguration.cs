// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityFrameworkPersistenceConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework
{
   using System;
   using System.Data.Common;
   using System.Data.Entity.Infrastructure;
   using System.Reflection;

   public interface IEntityFrameworkPersistenceConfiguration : IPersistenceConfiguration
   {
      bool IsEntityRegistered(Type entityType);

      IEntityFrameworkPersistenceConfiguration ProviderName(string providerName);

      IEntityFrameworkPersistenceConfiguration ConnectionString(string connectionString);

      IEntityFrameworkPersistenceConfiguration ConnectionStringName(string connectionStringName);

      IEntityFrameworkPersistenceConfiguration RegisterEntitiesFromAssemblyOf<T>();

      IEntityFrameworkPersistenceConfiguration RegisterEntitiesFromAssembly(Assembly assembly);

      DbCompiledModel CreateModel();

      DbConnection CreateConnection();
   }
}
