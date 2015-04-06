// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration
{
   using System.Collections.Generic;
   using System.Reflection;

   using Atlas.Core.DateTime;
   using Atlas.Core.Logging;
   using Atlas.Persistence;
   using Atlas.Persistence.NHibernate.ByteCode.Castle;
   using Atlas.Persistence.NHibernate.Configuration;
   using Atlas.Persistence.NHibernate.Configuration.ByCode;
   using Atlas.Persistence.NHibernate.Implementations;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory(
         IInterceptUnitOfWork interceptor = null,
         IAuditConfiguration auditConfiguration = null,
         IDateTime dateTime = null,
         IUserContext userContext = null)
      {
         var logger = new ConsoleLogger { DebugLoggingIsEnabled = false };

         var databaseConfigurer = new SqlServerDatabaseConfigurer()
            .ConnectionStringName("Persistence");

         var mapperConfigurer = new ByCodeMapperConfigurer()
            .RegisterEntitiesFromAssembly(Assembly.GetExecutingAssembly());

         var configuration = new NHibernateConfiguration(logger);
         configuration.RegisterConfigurer(databaseConfigurer);
         configuration.RegisterConfigurer(mapperConfigurer);
         configuration.RegisterConfigurer(new ProxyConfigurer<CastleProxyFactoryFactory>());

         var interceptors = new List<IInterceptUnitOfWork>();

         if (interceptor != null)
         {
            interceptors.Add(interceptor);
         }

         return new NHibernateUnitOfWork.Factory(configuration, interceptors.ToArray(), auditConfiguration, dateTime, userContext, logger);
      }
   }
}
