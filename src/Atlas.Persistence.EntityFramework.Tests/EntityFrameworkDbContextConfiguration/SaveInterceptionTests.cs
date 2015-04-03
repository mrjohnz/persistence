// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveInterceptionTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using Atlas.Persistence.Testing;

   public class SaveInterceptionTests : SaveInterceptionTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory(IInterceptUnitOfWork interceptor)
      {
         return Helper.CreateUnitOfWorkFactory(interceptor);
      }
   }
}
