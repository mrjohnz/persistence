//-----------------------------------------------------------------------
// <copyright file="ProxyTests.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.Other
{
   using System;

   using Atlas.Persistence;
   using Atlas.Persistence.NHibernate.Testing;
   using Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration;
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate.Proxy;

   using NUnit.Framework;

   public class ProxyTests
   {
      private static IUnitOfWorkFactory unitOfWorkFactory;

      [TestFixtureSetUp]
      public void SetupBeforeAllTests()
      {
         unitOfWorkFactory = Helper.CreateUnitOfWorkFactory();
      }

      [Test]
      public void ProxyReturnsNotNull()
      {
         var originalFoo = CreateFoo();

         using (var unitOfWork = CreateUnitOfWork())
         {
            var proxyFoo = unitOfWork.Proxy<Foo, long>(originalFoo.ID);

            Assert.IsNotNull(proxyFoo);
         }
      }

      [Test]
      public void ProxyReturnsProxy()
      {
         var originalFoo = CreateFoo();

         using (var unitOfWork = CreateUnitOfWork())
         {
            var proxyFoo = unitOfWork.Proxy<Foo, long>(originalFoo.ID);

            Assert.IsTrue(proxyFoo.IsProxy());
         }
      }

      [Test]
      public void ProxyReturnsUnloadedProxy()
      {
         var originalFoo = CreateFoo();

         using (var unitOfWork = CreateUnitOfWork())
         {
            var proxyFoo = unitOfWork.Proxy<Foo, long>(originalFoo.ID);

            Assert.IsFalse(proxyFoo.IsLoaded());
         }
      }

      [Test]
      public void TouchingProxyLoadsEntity()
      {
         var originalFoo = CreateFoo();

         using (var unitOfWork = CreateUnitOfWork())
         {
            var proxyFoo = unitOfWork.Proxy<Foo, long>(originalFoo.ID);

            proxyFoo.StringValue = "This should trigger the load";

            Assert.IsTrue(proxyFoo.IsLoaded());
         }
      }

      private static Foo CreateFoo()
      {
         using (var unitOfWork = CreateUnitOfWork())
         {
            var foo = new Foo { Guid = Guid.NewGuid() };

            unitOfWork.Add(foo);
            unitOfWork.Save();

            return foo;
         }
      }

      private static INHibernateUnitOfWork CreateUnitOfWork()
      {
         var unitOfWork = unitOfWorkFactory.Create();
         return (INHibernateUnitOfWork)unitOfWork;
      }
   }
}
