//-----------------------------------------------------------------------
// <copyright file="PartitionedEntityQueryableTestsBase.cs" company="Epworth Consulting Ltd.">
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

   // TODO: Need to check that filter is restrictive by adding more than loading.

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

         this.entityComparer = comparers;
      }

      [Test]
      public void ForeignKeyFilter()
      {
         var baseGuid = Guid.NewGuid();
         var fooGuid = Guid.NewGuid();
         long fooID;

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            var foo = new FooPartitioned { Guid = fooGuid };
            var baseClass = new BaseClassPartitioned { Foo = foo, Guid = baseGuid };

            unitOfWork.Add(baseClass);

            unitOfWork.Save();

            fooID = foo.ID;
         }

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
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
         this.WhereFooLong<FooPartitioned>(1, c => c.ID);
      }

      [Test]
      public void FooPartitionedWhereMultipleInt()
      {
         this.WhereFooLong<FooPartitioned>(3, c => c.ID);
      }

      [Test]
      public void FooPartitionedWhereSingleString()
      {
         this.WhereFooGuid<FooPartitioned>(1);
      }

      [Test]
      public void FooPartitionedWhereMultipleString()
      {
         this.WhereFooGuid<FooPartitioned>(3);
      }

      [Test]
      public void BaseClassPartitionedWhereSingleInt()
      {
         this.WhereBaseClassLong<BaseClassPartitioned>(1, c => c.ID);
      }

      [Test]
      public void BaseClassPartitionedWhereMultipleInt()
      {
         this.WhereBaseClassLong<BaseClassPartitioned>(3, c => c.ID);
      }

      [Test]
      public void BaseClassPartitionedWhereSingleString()
      {
         this.WhereBaseClassGuid<BaseClassPartitioned>(1);
      }

      [Test]
      public void BaseClassPartitionedWhereMultipleString()
      {
         this.WhereBaseClassGuid<BaseClassPartitioned>(3);
      }

      [Test]
      public void SubClassPartitionedWhereSingleInt()
      {
         this.WhereBaseClassLong<SubClassPartitioned>(1, c => c.ID);
      }

      [Test]
      public void SubClassPartitionedWhereMultipleInt()
      {
         this.WhereBaseClassLong<SubClassPartitioned>(3, c => c.ID);
      }

      [Test]
      public void SubClassPartitionedWhereSingleString()
      {
         this.WhereBaseClassGuid<SubClassPartitioned>(1);
      }

      [Test]
      public void SubClassPartitionedWhereMultipleString()
      {
         this.WhereBaseClassGuid<SubClassPartitioned>(3);
      }

      #endregion

      #region Non-partitioned Entities

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
      public void WhereSingleIntEnum()
      {
         this.WhereFooIntEnum<Foo>(1);
      }

      [Test]
      public void WhereMultipleIntEnum()
      {
         this.WhereFooIntEnum<Foo>(3);
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
      public void BaseClassWhereSingleIntEnum()
      {
         this.WhereBaseClassIntEnum<BaseClass>(1);
      }

      [Test]
      public void BaseClassWhereMultipleIntEnum()
      {
         this.WhereBaseClassIntEnum<BaseClass>(3);
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

      [Test]
      public void SubClassWhereSingleIntEnum()
      {
         this.WhereBaseClassIntEnum<SubClass>(1);
      }

      [Test]
      public void SubClassWhereMultipleIntEnum()
      {
         this.WhereBaseClassIntEnum<SubClass>(3);
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

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            IList<T> all = unitOfWork.Query<T>().ToList();

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
            IList<T> all = unitOfWork.Query<T>().ToList();

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

      private void WhereFooIntEnum<T>(int number)
         where T : class, IFoo, new()
      {
         var foos = this.AddFooItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(c => c.IntEnum, foos.Select(c => c.IntEnum)).ToList();

            this.entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private void WhereBaseClassIntEnum<T>(int number)
         where T : class, IBaseClass, new()
      {
         var foos = this.AddBaseClassItems<T>(number);

         Assert.AreEqual(number, foos.Count);

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
         {
            IList<T> result = unitOfWork.Query<T>().IsIn(c => c.IntEnum, foos.Select(c => c.IntEnum)).ToList();

            this.entityComparer.CompareList(foos.OrderBy(c => c.ID), result.OrderBy(c => c.ID));
         }
      }

      private IList<T> AddFooItems<T>(int number)
         where T : class, IFoo, new()
      {
         var items = new List<T>();

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
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

         using (IUnitOfWork unitOfWork = this.unitOfWorkFactory.Create())
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
