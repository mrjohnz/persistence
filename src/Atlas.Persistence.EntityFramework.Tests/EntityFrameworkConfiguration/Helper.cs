// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration
{
   using Atlas.Persistence;
   using Atlas.Persistence.EntityFramework.Implementations;
   using Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration;
   using Atlas.Persistence.Log4Net;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         var logger = Log4NetPersistenceLogger.FromConfig();
         var configuration = new EntityFrameworkConfiguration();

         configuration.ConnectionStringName("Persistence");
         configuration.ProviderName(EntityFrameworkConfiguration.SqlServerProviderName);
         configuration.RegisterEntitiesFromAssemblyOf<FooConfiguration>();

         return new EntityFrameworkUnitOfWork.Factory(configuration, logger);
      }
   }
}
