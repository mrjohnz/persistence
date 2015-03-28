// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration
{
   using Atlas.Core.Logging;
   using Atlas.Persistence;
   using Atlas.Persistence.EntityFramework.Implementations;
   using Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         var configuration = new EntityFrameworkConfiguration();

         configuration.ConnectionStringName("Persistence");
         configuration.ProviderName(EntityFrameworkConfiguration.SqlServerProviderName);
         configuration.RegisterEntitiesFromAssemblyOf<FooConfiguration>();

         return new EntityFrameworkUnitOfWork.Factory(configuration, new ConsoleLogger { DebugLoggingIsEnabled = false });
      }
   }
}
