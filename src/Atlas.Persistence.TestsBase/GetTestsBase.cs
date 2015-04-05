// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetTestsBase.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase
{
   using System;

   using Atlas.Persistence.TestsBase.Entities;

   using NUnit.Framework;

   public abstract class GetTestsBase
   {
      private IUnitOfWorkFactory unitOfWorkFactory;

      [TestFixtureSetUp]
      public void SetupBeforeAllTests()
      {
         this.unitOfWorkFactory = this.CreateUnitOfWorkFactory();
      }

      [Test]
      public void ReturnsNullIfFooDoesNotExist()
      {
         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = unitOfWork.Get<Foo, long>(0);

            Assert.That(foo, Is.Null);
         }
      }

      [Test]
      public void ReturnsNullIfGuidParentDoesNotExist()
      {
         var id = Guid.NewGuid();

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var guidParent = unitOfWork.Get<GuidParent, Guid>(id);

            Assert.That(guidParent, Is.Null);
         }
      }

      [Test]
      public void ReturnsFooIfFooExists()
      {
         long id;
         var guid = Guid.NewGuid();

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = new Foo { Guid = guid };

            unitOfWork.Add(foo);
            unitOfWork.Save();

            id = foo.ID;
         }

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = unitOfWork.Get<Foo, long>(id);

            Assert.That(foo, Is.Not.Null);
            Assert.That(foo.ID, Is.EqualTo(id));
            Assert.That(foo.Guid, Is.EqualTo(guid));
         }
      }

      [Test]
      public void ReturnsGuidParentIfGuidParentExists()
      {
         var id = Guid.NewGuid();

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var guidParent = new GuidParent { Guid = id, CreatedDateTime = DateTime.Now };

            unitOfWork.Add(guidParent);
            unitOfWork.Save();

            id = guidParent.Guid;
         }

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var guidParent = unitOfWork.Get<GuidParent, Guid>(id);

            Assert.That(guidParent, Is.Not.Null);
            Assert.That(guidParent.Guid, Is.EqualTo(id));
         }
      }

      protected abstract IUnitOfWorkFactory CreateUnitOfWorkFactory();
   }
}
