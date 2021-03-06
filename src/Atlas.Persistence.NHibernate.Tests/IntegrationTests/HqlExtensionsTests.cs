﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HqlExtensionsTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.IntegrationTests
{
   using System;
   using System.Linq;

   using Atlas.Core.Logging;
   using Atlas.Persistence.NHibernate.ByteCode.Castle;
   using Atlas.Persistence.NHibernate.Configuration;
   using Atlas.Persistence.NHibernate.Configuration.Fluent;
   using Atlas.Persistence.NHibernate.Implementations;
   using Atlas.Persistence.NHibernate.Implementations.Hql;
   using Atlas.Persistence.NHibernate.Testing;
   using Atlas.Persistence.NHibernate.Testing.Configuration;
   using Atlas.Persistence.NHibernate.Testing.Configuration.Fluent.Conventions;
   using Atlas.Persistence.TestsBase.Entities;

   using NUnit.Framework;

   public class HqlExtensionsTests
   {
      private INHibernatePersistenceConfiguration configuration;

      [SetUp]
      public void SetupBeforeEachTest()
      {
         var fluentMapperConfigurer = new FluentMapperConfigurer()
            .RegisterConvention<SQLiteXElementConvention>()
            .RegisterEntitiesFromAssemblyOf<HqlExtensionsTests>();

         this.configuration = new NHibernateConfiguration(new ConsoleLogger());
         this.configuration.RegisterConfigurer(new SQLiteDatabaseConfigurer());
         this.configuration.RegisterConfigurer(fluentMapperConfigurer);
         this.configuration.RegisterConfigurer(new ProxyConfigurer<CastleProxyFactoryFactory>());
      }

      [Test]
      public void IfExtensionMethodCanBeUsedWithinTheSumAggregateMethod()
      {
         using (var sessionFactory = this.configuration.CreateSessionFactory())
         {
            using (var unitOfWorkFactory = new SQLiteUnitOfWorkFactory(this.configuration, sessionFactory, null, null, null, null, new ConsoleLogger()))
            {
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 0, null)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 0, null)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 0, 1)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 0, 2)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 0, 3)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 1, null)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 1, 1)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 1, 2)", Guid.NewGuid()));

               using (var unitOfWork = unitOfWorkFactory.Create())
               {
                  var x =
                     unitOfWork.Query<Foo>().GroupBy(c => c.IntEnum).Select(c => new Tuple<IntEnum, int, int>(c.Key, c.Count(), c.Sum(d => Extensions.If(d.IntValue == null, 0, 1)))).ToList();

                  Assert.AreEqual(2, x.Count);
                  Assert.AreEqual(IntEnum.Zero, x[0].Item1);
                  Assert.AreEqual(5, x[0].Item2);
                  Assert.AreEqual(3, x[0].Item3);
                  Assert.AreEqual(IntEnum.One, x[1].Item1);
                  Assert.AreEqual(3, x[1].Item2);
                  Assert.AreEqual(2, x[1].Item3);
               }
            }
         }
      }

      [Test]
      public void IfExtensionMethodCanBeUsedWithReferenceProperty()
      {
         using (var sessionFactory = this.configuration.CreateSessionFactory())
         {
            using (var unitOfWorkFactory = new SQLiteUnitOfWorkFactory(this.configuration, sessionFactory, null, null, null, null, new ConsoleLogger()))
            {
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 0, null)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into Foo (Guid, IntEnum, IntValue) values ('{0}', 0, null)", Guid.NewGuid()));

               unitOfWorkFactory.ExecuteSql(string.Format("insert into BaseClass (Guid, IntEnum, FooID) values ('{0}', 0, null)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into BaseClass (Guid, IntEnum, FooID) values ('{0}', 0, 1)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into BaseClass (Guid, IntEnum, FooID) values ('{0}', 0, 2)", Guid.NewGuid()));
               unitOfWorkFactory.ExecuteSql(string.Format("insert into BaseClass (Guid, IntEnum, FooID) values ('{0}', 1, 2)", Guid.NewGuid()));

               using (var unitOfWork = unitOfWorkFactory.Create())
               {
                  var x =
                     unitOfWork.Query<BaseClass>()
                        .GroupBy(c => c.IntEnum)
                        .Select(c => new Tuple<IntEnum, int, int>(c.Key, c.Count(), c.Sum(d => Extensions.If(d.Foo == null, 0, 1))))
                        .ToList();

                  Assert.AreEqual(2, x.Count);
                  Assert.AreEqual(IntEnum.Zero, x[0].Item1);
                  Assert.AreEqual(3, x[0].Item2);
                  Assert.AreEqual(2, x[0].Item3);
                  Assert.AreEqual(IntEnum.One, x[1].Item1);
                  Assert.AreEqual(1, x[1].Item2);
                  Assert.AreEqual(1, x[1].Item3);
               }
            }
         }
      }
   }
}
