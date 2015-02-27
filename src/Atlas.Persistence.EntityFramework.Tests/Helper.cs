// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests
{
   using Atlas.Domain.Persistence.EntityFramework.Tests;
   using Atlas.Persistence;
   using Atlas.Persistence.EntityFramework.Implementations;

   using FakeItEasy;

   public static class Helper
   {
      public static IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         var logger = A.Fake<IPersistenceLogger>();
         var configuration = new EntityFrameworkConfiguration();

         configuration.ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=AtlasPersistenceTests;Integrated Security=True;MultipleActiveResultSets=True");
         configuration.ProviderName(EntityFrameworkConfiguration.SqlServerProviderName);
         configuration.RegisterEntitiesFromAssemblyOf<FooConfiguration>();

         return new EntityFrameworkUnitOfWork.Factory(configuration, logger);
      }
   }
}
