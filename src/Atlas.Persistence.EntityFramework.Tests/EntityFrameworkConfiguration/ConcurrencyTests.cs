// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrencyTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration
{
   using Atlas.Persistence.TestsBase;

   using NUnit.Framework;

   [TestFixture]
   public class ConcurrencyTests : ConcurrencyTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         return Helper.CreateUnitOfWorkFactory();
      }
   }
}
