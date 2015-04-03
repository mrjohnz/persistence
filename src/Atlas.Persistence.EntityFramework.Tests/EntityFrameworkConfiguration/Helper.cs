// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration
{
   using System.Collections.Generic;

   using Atlas.Core.Logging;
   using Atlas.Persistence;
   using Atlas.Persistence.EntityFramework.Implementations;
   using Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration.Configuration;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory(
         IInterceptUnitOfWork interceptor = null,
         IAuditConfiguration auditConfiguration = null,
         IDateTimeFacility dateTimeFacility = null,
         IUserContext userContext = null)
      {
         var configuration = new EntityFrameworkConfiguration();

         configuration.ConnectionStringName("Persistence");
         configuration.ProviderName(EntityFrameworkConfiguration.SqlServerProviderName);
         configuration.RegisterEntitiesFromAssemblyOf<FooConfiguration>();

         var interceptors = new List<IInterceptUnitOfWork>();

         if (interceptor != null)
         {
            interceptors.Add(interceptor);
         }

         if (auditConfiguration != null)
         {
            interceptors.Add(new EntityFrameworkAuditInterceptor(auditConfiguration, dateTimeFacility, userContext));
         }

         return new EntityFrameworkUnitOfWork.Factory(configuration, interceptors.ToArray(), new ConsoleLogger { DebugLoggingIsEnabled = false });
      }
   }
}
