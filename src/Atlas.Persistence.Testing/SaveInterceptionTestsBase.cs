// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveInterceptionTestsBase.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   using Atlas.Persistence.Testing.Entities;

   using NUnit.Framework;

   public abstract class SaveInterceptionTestsBase
   {
      private StubUnitOfWorkSaveInterceptor unitOfWorkSaveInterceptor;
      private IUnitOfWorkFactory unitOfWorkFactory;

      [SetUp]
      public void SetupBeforeEachTest()
      {
         this.unitOfWorkSaveInterceptor = new StubUnitOfWorkSaveInterceptor();
         this.unitOfWorkFactory = this.CreateUnitOfWorkFactory(this.unitOfWorkSaveInterceptor);
      }

      [Test]
      public void CreateFooShouldBeInterceptedAsCreated()
      {
         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = new Foo { Guid = Guid.NewGuid() };

            unitOfWork.Add(foo);
            unitOfWork.Save();

            Assert.That(this.unitOfWorkSaveInterceptor.AddEntities.Count, Is.EqualTo(1));
            Assert.That(this.unitOfWorkSaveInterceptor.AddEntities[0], Is.SameAs(foo));

            Assert.That(this.unitOfWorkSaveInterceptor.ModifyEntities.Count, Is.EqualTo(0));
            Assert.That(this.unitOfWorkSaveInterceptor.RemoveEntities.Count, Is.EqualTo(0));
         }
      }

      [Test]
      public void UpdateFooShouldBeInterceptedAsUpdated()
      {
         long fooId;

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = new Foo { Guid = Guid.NewGuid() };

            unitOfWork.Add(foo);
            unitOfWork.Save();

            fooId = foo.ID;
         }

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = unitOfWork.Query<Foo>().Single(c => c.ID == fooId);

            foo.Guid = Guid.NewGuid();
            unitOfWork.Save();

            Assert.That(this.unitOfWorkSaveInterceptor.ModifyEntities.Count, Is.EqualTo(1));
            Assert.That(this.unitOfWorkSaveInterceptor.ModifyEntities[0], Is.SameAs(foo));

            Assert.That(this.unitOfWorkSaveInterceptor.AddEntities.Count, Is.EqualTo(1));
            Assert.That(this.unitOfWorkSaveInterceptor.RemoveEntities.Count, Is.EqualTo(0));
         }
      }

      [Test]
      public void DeleteFooShouldBeInterceptedAsDeleted()
      {
         long fooId;

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = new Foo { Guid = Guid.NewGuid() };

            unitOfWork.Add(foo);
            unitOfWork.Save();

            fooId = foo.ID;
         }

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = unitOfWork.Query<Foo>().Single(c => c.ID == fooId);

            unitOfWork.Remove(foo);
            unitOfWork.Save();

            Assert.That(this.unitOfWorkSaveInterceptor.RemoveEntities.Count, Is.EqualTo(1));
            Assert.That(this.unitOfWorkSaveInterceptor.RemoveEntities[0], Is.SameAs(foo));

            Assert.That(this.unitOfWorkSaveInterceptor.AddEntities.Count, Is.EqualTo(1));
            Assert.That(this.unitOfWorkSaveInterceptor.ModifyEntities.Count, Is.EqualTo(0));
         }
      }

      [Test]
      public void UnchangedFooShouldNotBeIntercepted()
      {
         long fooId;

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = new Foo { Guid = Guid.NewGuid() };

            unitOfWork.Add(foo);
            unitOfWork.Save();

            fooId = foo.ID;
         }

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = unitOfWork.Query<Foo>().Single(c => c.ID == fooId);

            unitOfWork.Save();

            Assert.That(this.unitOfWorkSaveInterceptor.AddEntities.Count, Is.EqualTo(1));
            Assert.That(this.unitOfWorkSaveInterceptor.ModifyEntities.Count, Is.EqualTo(0));
            Assert.That(this.unitOfWorkSaveInterceptor.RemoveEntities.Count, Is.EqualTo(0));
         }
      }

      protected abstract IUnitOfWorkFactory CreateUnitOfWorkFactory(IInterceptUnitOfWork interceptor);

      private class StubUnitOfWorkSaveInterceptor : IInterceptUnitOfWork
      {
         public StubUnitOfWorkSaveInterceptor()
         {
            this.AddEntities = new List<object>();
            this.ModifyEntities = new List<object>();
            this.RemoveEntities = new List<object>();
         }

         public List<object> AddEntities { get; private set; }

         public List<object> ModifyEntities { get; private set; }

         public List<object> RemoveEntities { get; private set; }

         public void Add(object[] entities)
         {
            this.AddEntities.AddRange(entities);
         }

         public void Modify(object[] entities)
         {
            this.ModifyEntities.AddRange(entities);
         }

         public void Remove(object[] entities)
         {
            this.RemoveEntities.AddRange(entities);
         }
      }
   }
}
