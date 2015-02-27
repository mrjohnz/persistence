//-----------------------------------------------------------------------
// <copyright file="PartitionedEntityQueryableTestsBase.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   using Atlas.Persistence.Testing.Entities;

   using NUnit.Framework;

   //TODO: Need to check that filter is restrictive by adding more than loading.

   public abstract class PartitionedEntityQueryableTestsBase
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
         comparers.Add<FooPartitioned>(Compare);
         comparers.Add<BaseClassPartitioned>(Compare);
         comparers.Add<SubClassPartitioned>(Compare);

         entityComparer = comparers;
      }

      [Test]
      public void ForeignKeyFilter()
      {
         var baseGuid = Guid.NewGuid();
         var fooGuid = Guid.NewGuid();
         long fooID;

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            var foo = new FooPartitioned() { Guid = fooGuid };
            var baseClass = new BaseClassPartitioned() { Foo = foo, Guid = baseGuid };

            unitOfWork.Add(baseClass);

            unitOfWork.Save();

            fooID = foo.ID;
         }

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            var baseClassByID = unitOfWork.Query<BaseClassPartitioned>().SingleOrDefault(c => c.Foo.ID == fooID);
            var baseClassByGuid = unitOfWork.Query<BaseClassPartitioned>().SingleOrDefault(c => c.Foo.Guid == fooGuid);

            Assert.IsNotNull(baseClassByID);
            Assert.IsNotNull(baseClassByGuid);

            Assert.AreEqual(baseGuid, baseClassByID.Guid);
            Assert.AreEqual(baseGuid, baseClassByGuid.Guid);
         }
      }

      #region Partitioned Entities

      [Test]
      public void FooPartitionedWhereSingleInt()
      {
         WhereFooLong<FooPartitioned>(1, c => c.ID);
      }

      [Test]
      public void FooPartitionedWhereMultipleInt()
      {
         WhereFooLong<FooPartitioned>(3, c => c.ID);
      }

      [Test]
      public void FooPartitionedWhereSingleString()
      {
         WhereFooGuid<FooPartitioned>(1);
      }

      [Test]
      public void FooPartitionedWhereMultipleString()
      {
         WhereFooGuid<FooPartitioned>(3);
      }

      [Test]
      public void BaseClassPartitionedWhereSingleInt()
      {
         WhereBaseClassLong<BaseClassPartitioned>(1, c => c.ID);
      }

      [Test]
      public void BaseClassPartitionedWhereMultipleInt()
      {
         WhereBaseClassLong<BaseClassPartitioned>(3, c => c.ID);
      }

      [Test]
      public void BaseClassPartitionedWhereSingleString()
      {
         WhereBaseClassGuid<BaseClassPartitioned>(1);
      }

      [Test]
      public void BaseClassPartitionedWhereMultipleString()
      {
         WhereBaseClassGuid<BaseClassPartitioned>(3);
      }

      [Test]
      public void SubClassPartitionedWhereSingleInt()
      {
         WhereBaseClassLong<SubClassPartitioned>(1, c => c.ID);
      }

      [Test]
      public void SubClassPartitionedWhereMultipleInt()
      {
         WhereBaseClassLong<SubClassPartitioned>(3, c => c.ID);
      }

      [Test]
      public void SubClassPartitionedWhereSingleString()
      {
         WhereBaseClassGuid<SubClassPartitioned>(1);
      }

      [Test]
      public void SubClassPartitionedWhereMultipleString()
      {
         WhereBaseClassGuid<SubClassPartitioned>(3);
      }

      #endregion

      #region Non-partitioned Entities

      [Test]
      public void WhereSingleInt()
      {
         WhereFooLong<Foo>(1, c => c.ID);
      }

      [Test]
      public void WhereMultipleInt()
      {
         WhereFooLong<Foo>(3, c => c.ID);
      }

      [Test]
      public void WhereSingleString()
      {
         WhereFooGuid<Foo>(1);
      }

      [Test]
      public void WhereMultipleString()
      {
         WhereFooGuid<Foo>(3);
      }

      [Test]
      public void WhereSingleIntEnum()
      {
         WhereFooIntEnum<Foo>(1);
      }

      [Test]
      public void WhereMultipleIntEnum()
      {
         WhereFooIntEnum<Foo>(3);
      }

      [Test]
      public void BaseClassWhereSingleInt()
      {
         WhereBaseClassLong<BaseClass>(1, c => c.ID);
      }

      [Test]
      public void BaseClassWhereMultipleInt()
      {
         WhereBaseClassLong<BaseClass>(3, c => c.ID);
      }

      [Test]
      public void BaseClassWhereSingleString()
      {
         WhereBaseClassGuid<BaseClass>(1);
      }

      [Test]
      public void BaseClassWhereMultipleString()
      {
         WhereBaseClassGuid<BaseClass>(3);
      }

      [Test]
      public void BaseClassWhereSingleIntEnum()
      {
         WhereBaseClassIntEnum<BaseClass>(1);
      }

      [Test]
      public void BaseClassWhereMultipleIntEnum()
      {
         WhereBaseClassIntEnum<BaseClass>(3);
      }

      [Test]
      public void SubClassWhereSingleInt()
      {
         WhereBaseClassLong<SubClass>(1, c => c.ID);
      }

      [Test]
      public void SubClassWhereMultipleInt()
      {
         WhereBaseClassLong<SubClass>(3, c => c.ID);
      }

      [Test]
      public void SubClassWhereSingleString()
      {
         WhereBaseClassGuid<SubClass>(1);
      }

      [Test]
      public void SubClassWhereMultipleString()
      {
         WhereBaseClassGuid<SubClass>(3);
      }

      [Test]
      public void SubClassWhereSingleIntEnum()
      {
         WhereBaseClassIntEnum<SubClass>(1);
      }

      [Test]
      public void SubClassWhereMultipleIntEnum()
      {
         WhereBaseClassIntEnum<SubClass>(3);
      }

      #endregion

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

      private static bool Compare(FooPartitioned expected, FooPartitioned actual)
      {
         return expected.ID == actual.ID && expected.Guid == actual.Guid && expected.PartitionGuid == actual.PartitionGuid;
      }

      private static bool Compare(BaseClassPartitioned expected, BaseClassPartitioned actual)
      {
         return expected.ID == actual.ID && expected.Guid == actual.Guid && expected.PartitionGuid == actual.PartitionGuid;
      }

      private static bool Compare(SubClassPartitioned expected, SubClassPartitioned actual)
      {
         return expected.ID == actual.ID && expected.Guid == actual.Guid && expected.PartitionGuid == actual.PartitionGuid && expected.Name == actual.Name;
      }

      private void WhereFooLong<T>(int number, Expression<Func<T, long>> idProperty)
         where T : class, IFoo, new()
      {
         IList<T> foos = this.AddFooItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            IList<T> all = unitOfWork.Query<T>().ToList();

            IList<T> result = unitOfWork.Query<T>().IsIn(idProperty, foos.Select(c => c.ID)).ToList();

            entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereBaseClassLong<T>(int number, Expression<Func<T, long>> idProperty)
         where T : class, IBaseClass, new()
      {
         IList<T> foos = this.AddBaseClassItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            IList<T> all = unitOfWork.Query<T>().ToList();

            IList<T> result = unitOfWork.Query<T>().IsIn(idProperty, foos.Select(c => c.ID)).ToList();

            entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereFooGuid<T>(int number)
         where T : class, IFoo, new()
      {
         IList<T> foos = this.AddFooItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(c => c.Guid, foos.Select(c => c.Guid)).ToList();

            entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereBaseClassGuid<T>(int number)
         where T : class, IBaseClass, new()
      {
         IList<T> foos = this.AddBaseClassItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(c => c.Guid, foos.Select(c => c.Guid)).ToList();

            entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereFooIntEnum<T>(int number)
         where T : class, IFoo, new()
      {
         var foos = this.AddFooItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(c => c.IntEnum, foos.Select(c => c.IntEnum)).ToList();

            entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereBaseClassIntEnum<T>(int number)
         where T : class, IBaseClass, new()
      {
         var foos = this.AddBaseClassItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(c => c.IntEnum, foos.Select(c => c.IntEnum)).ToList();

            entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private IList<T> AddFooItems<T>(int number)
         where T : class, IFoo, new()
      {
         var items = new List<T>();

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            for (int i = 1; i <= number; i++)
            {
               T item = new T { Guid = Guid.NewGuid(), IntEnum = IntEnum.One };

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
         var items = new List<T>();

         using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
         {
            for (int i = 1; i <= number; i++)
            {
               T item = new T { Guid = Guid.NewGuid(), IntEnum = IntEnum.One };

               unitOfWork.Add(item);
               items.Add(item);
            }

            unitOfWork.Save();
         }

         return items;
      }
   }
}
