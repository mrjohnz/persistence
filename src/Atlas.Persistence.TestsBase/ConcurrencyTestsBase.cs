//-----------------------------------------------------------------------
// <copyright file="ConcurrencyTestsBase.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase
{
   using System.Linq;

   using Atlas.Persistence.TestsBase.Entities;

   using NUnit.Framework;

   public abstract class ConcurrencyTestsBase
   {
      private IUnitOfWorkFactory unitOfWorkFactory;

      [TestFixtureSetUp]
      public void SetupBeforeAllTests()
      {
         this.unitOfWorkFactory = this.CreateUnitOfWorkFactory();
      }

      [Test]
      public void AddEntityWithVersion()
      {
         this.AddOptimistic();
      }

      [Test]
      public void UpdateEntityWithVersion()
      {
         var id = this.AddOptimistic();

         this.UpdateOptimistic(id, "Updated");
      }

      [Test]
      public void SaveThrowsConcurrencyExceptionIfChangedByAnotherUnitOfWork()
      {
         var id = this.AddOptimistic();

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var optimistic = unitOfWork.Query<Optimistic>()
               .Single(c => c.ID == id);

            optimistic.StringValue = "Fail";

            this.UpdateOptimistic(id, "Conflict");

            Assert.That(() => unitOfWork.Save(), Throws.InstanceOf<ConcurrencyException>().With.Message.ContainsSubstring("Data has been changed by another transaction"));
         }
      }

      protected abstract IUnitOfWorkFactory CreateUnitOfWorkFactory();

      private long AddOptimistic()
      {
         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var optimistic = new Optimistic();

            unitOfWork.Add(optimistic);

            unitOfWork.Save();

            return optimistic.ID;
         }
      }

      private void UpdateOptimistic(long id, string stringValue)
      {
         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var optimistic = unitOfWork.Query<Optimistic>()
               .Single(c => c.ID == id);

            optimistic.StringValue = stringValue;

            unitOfWork.Save();
         }
      }
   }
}
