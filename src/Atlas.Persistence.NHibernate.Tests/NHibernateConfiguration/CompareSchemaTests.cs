//-----------------------------------------------------------------------
// <copyright file="CompareSchemaTests.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration
{
   using System.Configuration;
   using System.Reflection;

   using Atlas.Core.Logging;
   using Atlas.Persistence.NHibernate.ByteCode.Unity;
   using Atlas.Persistence.NHibernate.Configuration;
   using Atlas.Persistence.NHibernate.Configuration.ByCode;
   using Atlas.Persistence.NHibernate.Configuration.Fluent;
   using Atlas.Persistence.NHibernate.Implementations;
   using Atlas.Persistence.Testing.SqlServer;

   using NUnit.Framework;

   public class CompareSchemaTests
   {
      [Test]
      [Category("CompareSchema")]
      [Timeout(300000)]
      public void CompareByCodeSchema()
      {
         var subSetConnectionString = ConfigurationManager.ConnectionStrings["PersistenceNHByCode"].ConnectionString;

         var databaseConfigurer = new SqlServerDatabaseConfigurer()
            .ConnectionString(subSetConnectionString);

         var mapperConfigurer = new ByCodeMapperConfigurer()
            .RegisterEntitiesFromAssembly(Assembly.GetExecutingAssembly())
            .RegisterConvention<NHibernate.Configuration.ByCode.Conventions.DateTime2Convention>();

         this.CompareSchema(
            subSetConnectionString,
            databaseConfigurer,
            mapperConfigurer,
            new ProxyConfigurer<UnityProxyFactoryFactory>());
      }

      [Test]
      [Category("CompareSchema")]
      [Timeout(300000)]
      public void CompareFluentSchema()
      {
         var subSetConnectionString = ConfigurationManager.ConnectionStrings["PersistenceNHFluent"].ConnectionString;

         var databaseConfigurer = new SqlServerDatabaseConfigurer()
            .ConnectionString(subSetConnectionString);

         var mapperConfigurer = new FluentMapperConfigurer()
            .RegisterEntitiesFromAssembly(Assembly.GetExecutingAssembly())
            .RegisterConvention<NHibernate.Configuration.Fluent.Conventions.DateTime2Convention>()
            .RegisterConvention<NHibernate.Configuration.Fluent.Conventions.VersionConvention>()
            .RegisterConvention<NHibernate.Configuration.Fluent.Conventions.XElementConvention>();

         this.CompareSchema(
            subSetConnectionString,
            databaseConfigurer,
            mapperConfigurer,
            new ProxyConfigurer<UnityProxyFactoryFactory>());
      }

      //[Ignore] // Not yet fully implemented
      //[Test]
      //[Category("CompareSchema")]
      //[Timeout(300000)]
      //public void CompareFluentAutoMapSchema()
      //{
      //   var subSetConnectionString = ConfigurationManager.ConnectionStrings["PersistenceNHFluentAuto"].ConnectionString;

      //   var databaseConfigurer = new SqlServerDatabaseConfigurer()
      //      .ConnectionString(subSetConnectionString);

      //   var mapperConfigurer = new FluentAutoMapperConfigurer()
      //      .AutoMappingConfiguration(new SQLiteAtlasAutoMappingConfiguration())
      //      .AutoMapEntitiesFromAssembly(Assembly.GetExecutingAssembly())
      //      .RegisterConvention<NHibernate.Configuration.Fluent.Conventions.PropertyConvention>()
      //      .RegisterConvention<NHibernate.Configuration.Fluent.Conventions.DateTime2Convention>()
      //      .RegisterConvention<NHibernate.Configuration.Fluent.Conventions.VersionConvention>()
      //      .RegisterConvention<NHibernate.Configuration.Fluent.Conventions.XElementConvention>();

      //   this.CompareSchema(
      //      subSetConnectionString,
      //      databaseConfigurer,
      //      mapperConfigurer,
      //      new ProxyConfigurer<UnityProxyFactoryFactory>());
      //}

      private void CompareSchema(string subSetConnectionString, params INHibernateConfigurer[] configurers)
      {
         var superSetConnectionString = ConfigurationManager.ConnectionStrings["Persistence"].ConnectionString;

         SqlServerSchema.Prepare(subSetConnectionString);

         var configuration = new NHibernateConfiguration(new ConsoleLogger { DebugLoggingIsEnabled = false });

         foreach (var configurer in configurers)
         {
            configuration.RegisterConfigurer(configurer);
         }

         configuration.CreateSchema();

         SqlServerSchema.AssertContained(new ConsoleLogger(), superSetConnectionString, subSetConnectionString, false);
      }
   }
}
