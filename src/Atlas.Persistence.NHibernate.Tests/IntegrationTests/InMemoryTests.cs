// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.IntegrationTests
{
   using System.Reflection;

   using Atlas.Core.Logging;
   using Atlas.Persistence.NHibernate.ByteCode.Castle;
   using Atlas.Persistence.NHibernate.Configuration;
   using Atlas.Persistence.NHibernate.Configuration.Fluent;
   using Atlas.Persistence.NHibernate.Implementations;
   using Atlas.Persistence.NHibernate.Testing;
   using Atlas.Persistence.NHibernate.Testing.Configuration;
   using Atlas.Persistence.NHibernate.Testing.Configuration.Fluent.Conventions;
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate;
   using global::NHibernate.Exceptions;

   using NUnit.Framework;

   public class InMemoryTests
   {
      private INHibernatePersistenceConfiguration configuration;
      private ISessionFactory sessionFactory;

      [TestFixtureSetUp]
      public void SetupOnceBeforeAllTests()
      {
         var logger = new ConsoleLogger();

         var databaseConfigurer = new SQLiteDatabaseConfigurer();

         var mapperConfigurer = new FluentMapperConfigurer()
            .RegisterConvention<SQLiteXElementConvention>()
            .RegisterEntitiesFromAssembly(Assembly.GetExecutingAssembly());

         this.configuration = new NHibernateConfiguration(logger);
         this.configuration.RegisterConfigurer(databaseConfigurer);
         this.configuration.RegisterConfigurer(mapperConfigurer);
         this.configuration.RegisterConfigurer(new ProxyConfigurer<CastleProxyFactoryFactory>());

         this.sessionFactory = this.configuration.CreateSessionFactory();
      }

      [TestFixtureTearDown]
      public void TeardownOnceAfterAllTests()
      {
         this.sessionFactory.Dispose();
      }

      [Test]
      public void InsertingDuplicateBarNameInSameUnitOfWorkThrowsException()
      {
         using (var unitOfWorkFactory = new SQLiteUnitOfWorkFactory(this.configuration, this.sessionFactory, null, null, null, null, new ConsoleLogger { DebugLoggingIsEnabled = false }))
         {
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
               unitOfWork.Add(new Bar { Name = "myName" });

               Assert.That(() => unitOfWork.Add(new Bar { Name = "myName" }), Throws.InstanceOf<GenericADOException>());
            }
         }
      }

      [Test]
      public void InsertingDuplicateBarNameInDifferentUnitOfWorkSameFactoryThrowsException()
      {
         using (var unitOfWorkFactory = new SQLiteUnitOfWorkFactory(this.configuration, this.sessionFactory, null, null, null, null, new ConsoleLogger { DebugLoggingIsEnabled = false }))
         {
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
               unitOfWork.Add(new Bar { Name = "myName" });
               unitOfWork.Save();
            }

            using (var unitOfWork = unitOfWorkFactory.Create())
            {
               Assert.That(() => unitOfWork.Add(new Bar { Name = "myName" }), Throws.InstanceOf<GenericADOException>());
            }
         }
      }

      [Test]
      public void InsertingDuplicateBarNameInDifferentFactoryDoesNotThrowException()
      {
         using (var unitOfWorkFactory = new SQLiteUnitOfWorkFactory(this.configuration, this.sessionFactory, null, null, null, null, new ConsoleLogger { DebugLoggingIsEnabled = false }))
         {
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
               unitOfWork.Add(new Bar { Name = "myName" });
               unitOfWork.Save();
            }
         }

         using (var unitOfWorkFactory = new SQLiteUnitOfWorkFactory(this.configuration, this.sessionFactory, null, null, null, null, new ConsoleLogger { DebugLoggingIsEnabled = false }))
         {
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
               Assert.That(() => unitOfWork.Add(new Bar { Name = "myName" }), Throws.Nothing);
            }
         }
      }
   }
}
