// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using System.Collections.Generic;

   using Atlas.Core.Logging;
   using Atlas.Persistence;
   using Atlas.Persistence.EntityFramework.Implementations;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory(
         IInterceptUnitOfWork interceptor = null,
         IAuditConfiguration auditConfiguration = null,
         IDateTimeFacility dateTimeFacility = null,
         IUserContext userContext = null)
      {
         var configuration = new EntityFrameworkDbContextConfiguration<CompareContext>(connectionStringOrName => new CompareContext(connectionStringOrName));

         configuration.ConnectionStringName("Persistence");

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
