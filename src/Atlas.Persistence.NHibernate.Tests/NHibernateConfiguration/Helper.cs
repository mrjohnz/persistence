// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration
{
   using System.Reflection;

   using Atlas.Core.Logging;
   using Atlas.Persistence;
   using Atlas.Persistence.NHibernate.ByteCode.Unity;
   using Atlas.Persistence.NHibernate.Configuration;
   using Atlas.Persistence.NHibernate.Configuration.ByCode;
   using Atlas.Persistence.NHibernate.Implementations;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         var logger = new ConsoleLogger { DebugLoggingIsEnabled = false };

         var databaseConfigurer = new SqlServerDatabaseConfigurer()
            .ConnectionStringName("Persistence");

         var mapperConfigurer = new ByCodeMapperConfigurer()
            .RegisterEntitiesFromAssembly(Assembly.GetExecutingAssembly());

         var configuration = new NHibernateConfiguration(logger);
         configuration.RegisterConfigurer(databaseConfigurer);
         configuration.RegisterConfigurer(mapperConfigurer);
         configuration.RegisterConfigurer(new ProxyConfigurer<UnityProxyFactoryFactory>());

         return new NHibernateUnitOfWork.Factory(configuration, logger);
      }
   }
}
