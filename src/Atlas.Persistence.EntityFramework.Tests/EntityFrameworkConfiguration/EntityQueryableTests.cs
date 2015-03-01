// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityQueryableTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration
{
   using Atlas.Persistence.Testing;

   using NUnit.Framework;

   [TestFixture]
   public class EntityQueryableTests : EntityQueryableTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         return Helper.CreateUnitOfWorkFactory();
      }
   }
}
