// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using Atlas.Persistence.TestsBase;

   using NUnit.Framework;

   [TestFixture]
   public class GetTests : GetTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         return Helper.CreateUnitOfWorkFactory();
      }
   }
}
