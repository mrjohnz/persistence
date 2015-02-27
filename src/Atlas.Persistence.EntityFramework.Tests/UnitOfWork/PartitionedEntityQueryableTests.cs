// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartitionedEntityQueryableTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.UnitOfWork
{
   using Atlas.Persistence.Testing;

   public class PartitionedEntityQueryableTests : PartitionedEntityQueryableTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         return Helper.CreateUnitOfWorkFactory();
      }
   }
}
