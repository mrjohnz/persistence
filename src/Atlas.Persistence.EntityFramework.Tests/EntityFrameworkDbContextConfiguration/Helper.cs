// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using Atlas.Persistence;
   using Atlas.Persistence.EntityFramework.Implementations;
   using Atlas.Persistence.Log4Net;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         var logger = Log4NetPersistenceLogger.FromConfig();
         var configuration = new EntityFrameworkDbContextConfiguration<CompareContext>(connectionStringOrName => new CompareContext(connectionStringOrName));

         configuration.ConnectionStringName("Persistence");

         return new EntityFrameworkUnitOfWork.Factory(configuration, logger);
      }
   }
}
