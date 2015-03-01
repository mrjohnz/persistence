// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartitionedEntityQueryableTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkConfiguration
{
   using Atlas.Persistence.Testing;

   using NUnit.Framework;

   [TestFixture]
   public class PartitionedEntityQueryableTests : PartitionedEntityQueryableTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         return Helper.CreateUnitOfWorkFactory();
      }
   }
}
