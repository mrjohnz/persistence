//-----------------------------------------------------------------------
// <copyright file="EntityQueryableTestsBase.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   using Atlas.Persistence.Testing;
   using Atlas.Persistence.TestsBase.Entities;

   using NUnit.Framework;

   // TODO: Add tests for boolean fields. NH 3.1 includes Boolean type rather than having to write custom user type

   public abstract class EntityQueryableTestsBase
   {
      private IUnitOfWorkFactory unitOfWorkFactory;
      private EntityComparer entityComparer;

      [TestFixtureSetUp]
      public void SetupBeforeAllTests()
      {
         this.unitOfWorkFactory = this.CreateUnitOfWorkFactory();

         var comparers = new EntityComparer();

         comparers.Add<Foo>(Compare);
         comparers.Add<BaseClass>(Compare);
         comparers.Add<SubClass>(Compare);

         this.entityComparer = comparers;
      }

      [Test]
      public void ForeignKeyFilter()
      {
         Guid baseGuid = Guid.NewGuid();
         Guid fooGuid = Guid.NewGuid();
         long fooID;

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            Foo foo = new Foo { Guid = fooGuid };
            BaseClass baseClass = new BaseClass { Foo = foo, Guid = baseGuid };

            unitOfWork.Add(baseClass);

            unitOfWork.Save();

            fooID = foo.ID;
         }

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            BaseClass baseClassByID = unitOfWork.Query<BaseClass>().SingleOrDefault(c => c.Foo.ID == fooID);
            BaseClass baseClassByGuid = unitOfWork.Query<BaseClass>().SingleOrDefault(c => c.Foo.Guid == fooGuid);

            Assert.IsNotNull(baseClassByID);
            Assert.IsNotNull(baseClassByGuid);

            Assert.AreEqual(baseGuid, baseClassByID.Guid);
            Assert.AreEqual(baseGuid, baseClassByGuid.Guid);
         }
      }

      [Test]
      public void WhereSingleInt()
      {
         this.WhereFooLong<Foo>(1, c => c.ID);
      }

      [Test]
      public void WhereMultipleInt()
      {
         this.WhereFooLong<Foo>(3, c => c.ID);
      }

      [Test]
      public void WhereSingleString()
      {
         this.WhereFooGuid<Foo>(1);
      }

      [Test]
      public void WhereMultipleString()
      {
         this.WhereFooGuid<Foo>(3);
      }

      [Test]
      public void BaseClassWhereSingleInt()
      {
         this.WhereBaseClassLong<BaseClass>(1, c => c.ID);
      }

      [Test]
      public void BaseClassWhereMultipleInt()
      {
         this.WhereBaseClassLong<BaseClass>(3, c => c.ID);
      }

      [Test]
      public void BaseClassWhereSingleString()
      {
         this.WhereBaseClassGuid<BaseClass>(1);
      }

      [Test]
      public void BaseClassWhereMultipleString()
      {
         this.WhereBaseClassGuid<BaseClass>(3);
      }

      [Test]
      public void SubClassWhereSingleInt()
      {
         this.WhereBaseClassLong<SubClass>(1, c => c.ID);
      }

      [Test]
      public void SubClassWhereMultipleInt()
      {
         this.WhereBaseClassLong<SubClass>(3, c => c.ID);
      }

      [Test]
      public void SubClassWhereSingleString()
      {
         this.WhereBaseClassGuid<SubClass>(1);
      }

      [Test]
      public void SubClassWhereMultipleString()
      {
         this.WhereBaseClassGuid<SubClass>(3);
      }

      protected abstract IUnitOfWorkFactory CreateUnitOfWorkFactory();

      private static bool Compare(Foo expected, Foo actual)
      {
         return expected.ID == actual.ID && expected.Guid == actual.Guid;
      }

      private static bool Compare(BaseClass expected, BaseClass actual)
      {
         return expected.ID == actual.ID && expected.Guid == actual.Guid;
      }

      private static bool Compare(SubClass expected, SubClass actual)
      {
         return expected.ID == actual.ID && expected.Guid == actual.Guid && expected.Name == actual.Name;
      }

      private void WhereFooLong<T>(int number, Expression<Func<T, long>> idProperty)
         where T : class, IFoo, new()
      {
         IList<T> foos = this.AddFooItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(idProperty, foos.Select(c => c.ID)).ToList();

            this.entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereBaseClassLong<T>(int number, Expression<Func<T, long>> idProperty)
         where T : class, IBaseClass, new()
      {
         IList<T> foos = this.AddBaseClassItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(idProperty, foos.Select(c => c.ID)).ToList();

            this.entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereFooGuid<T>(int number)
         where T : class, IFoo, new()
      {
         IList<T> foos = this.AddFooItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(c => c.Guid, foos.Select(c => c.Guid)).ToList();

            this.entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereBaseClassGuid<T>(int number)
         where T : class, IBaseClass, new()
      {
         IList<T> foos = this.AddBaseClassItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(c => c.Guid, foos.Select(c => c.Guid)).ToList();

            this.entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private IList<T> AddFooItems<T>(int number)
         where T : class, IFoo, new()
      {
         List<T> items = new List<T>();

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            for (int i = 1; i <= number; i++)
            {
               T item = new T { Guid = Guid.NewGuid() };

               unitOfWork.Add(item);
               items.Add(item);
            }

            unitOfWork.Save();
         }

         return items;
      }

      private IList<T> AddBaseClassItems<T>(int number)
         where T : class, IBaseClass, new()
      {
         List<T> items = new List<T>();

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            for (int i = 1; i <= number; i++)
            {
               T item = new T { Guid = Guid.NewGuid() };

               unitOfWork.Add(item);
               items.Add(item);
            }

            unitOfWork.Save();
         }

         return items;
      }
   }
}
