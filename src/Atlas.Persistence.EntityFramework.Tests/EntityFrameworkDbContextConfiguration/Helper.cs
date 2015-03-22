// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using Atlas.Core.Logging;
   using Atlas.Persistence;
   using Atlas.Persistence.EntityFramework.Implementations;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         var configuration = new EntityFrameworkDbContextConfiguration<CompareContext>(connectionStringOrName => new CompareContext(connectionStringOrName));

         configuration.ConnectionStringName("Persistence");

         return new EntityFrameworkUnitOfWork.Factory(configuration, new ConsoleLogger());
      }
   }
}
