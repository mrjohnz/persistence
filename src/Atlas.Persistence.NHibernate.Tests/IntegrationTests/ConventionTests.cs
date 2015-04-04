// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConventionTests.cs" company="Epworth Consulting Ltd.">
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
   using Atlas.Persistence.NHibernate.Configuration.Fluent.Conventions;
   using Atlas.Persistence.NHibernate.Implementations;
   using Atlas.Persistence.NHibernate.Testing;
   using Atlas.Persistence.NHibernate.Testing.Configuration;
   using Atlas.Persistence.NHibernate.Testing.Configuration.Fluent.Conventions;
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate;

   using NUnit.Framework;

   public class ConventionTests
   {
      private FluentMapperConfigurer fluentMapperConfigurer;
      private INHibernatePersistenceConfiguration persistenceConfiguration;

      [SetUp]
      public void SetupBeforeEachTest()
      {
         this.fluentMapperConfigurer = new FluentMapperConfigurer()
            .RegisterConvention<SQLiteXElementConvention>()
            .RegisterEntitiesFromAssemblyOf<ConventionTests>();

         this.persistenceConfiguration = new NHibernateConfiguration(new ConsoleLogger());
         this.persistenceConfiguration.RegisterConfigurer(new SQLiteDatabaseConfigurer());
         this.persistenceConfiguration.RegisterConfigurer(this.fluentMapperConfigurer);
         this.persistenceConfiguration.RegisterConfigurer(new ProxyConfigurer<CastleProxyFactoryFactory>());
      }

      [Test]
      public void ExceptionThrownIfIgnoreVersionConventionNotRegistered()
      {
         using (var unitOfWorkFactory = new SQLiteUnitOfWorkFactory(this.persistenceConfiguration, null, null, null, null, new ConsoleLogger()))
         {
            using (var unitOfWork = unitOfWorkFactory.Create())
            {
               var safeUnitOfWork = unitOfWork;

               var optimistic = new Optimistic { StringValue = "test" };

               Assert.That(() => safeUnitOfWork.Add(optimistic), Throws.InstanceOf<PropertyValueException>());
            }
         }
      }

      [Test]
      public void DateTime2ConventionStoresDateTimeWithMilliseconds()
      {
         this.fluentMapperConfigurer.RegisterConvention<DateTime2Convention>();

         var dateTime = new DateTime(2012, 12, 19, 21, 56, 7, 456);

         using (var unitOfWorkFactory = new SQLiteUnitOfWorkFactory(this.persistenceConfiguration, null, null, null, null, new ConsoleLogger()))
         {
            long fooId;

            using (var unitOfWork = unitOfWorkFactory.Create())
            {
               var foo = new Foo { DateTimeValue = dateTime };

               unitOfWork.Add(foo);
               unitOfWork.Save();

               fooId = foo.ID;
            }

            using (var unitOfWork = unitOfWorkFactory.Create())
            {
               var foo = unitOfWork.Query<Foo>()
                  .Single(c => c.ID == fooId);

               Assert.AreEqual(dateTime, foo.DateTimeValue);
            }
         }
      }
   }
}
