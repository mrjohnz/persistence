// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations
{
   using System;
   using System.Collections.Generic;
   using System.Data;

   using Atlas.Core.Logging;
   using Atlas.Persistence.NHibernate;
   using Atlas.Persistence.NHibernate.Implementations.Hql;

   using global::NHibernate;
   using global::NHibernate.Cfg;
   using global::NHibernate.Dialect;
   using global::NHibernate.Tool.hbm2ddl;

   public class NHibernateConfiguration : INHibernatePersistenceConfiguration
   {
      private const string DialectKey = "dialect";

      private readonly IList<INHibernateConfigurer> configurers = new List<INHibernateConfigurer>();
      private readonly ILogger logger;

      public NHibernateConfiguration(ILogger logger)
      {
         ThrowIf.ArgumentIsNull(logger, "persistenceLogger");

         this.logger = logger;
      }

      public string[] SchemaCreationScript()
      {
         var configuration = this.CreateConfiguration();

         var dialectTypeName = configuration.GetProperty(DialectKey);
         var dialectType = Type.GetType(dialectTypeName, true);
         var dialect = (Dialect)Activator.CreateInstance(dialectType);

         return configuration.GenerateSchemaCreationScript(dialect);
      }

      public void CreateSchema()
      {
         var configuration = this.CreateConfiguration();

         var schemaExport = new SchemaExport(configuration);
         schemaExport.Create(false, true);
      }

      public void CreateSchema(IDbConnection connection)
      {
         var configuration = this.CreateConfiguration();

         var schemaExport = new SchemaExport(configuration);
         schemaExport.Execute(false, true, false, connection, null);
      }

      public INHibernatePersistenceConfiguration RegisterConfigurer(INHibernateConfigurer configurer)
      {
         this.configurers.Add(configurer);

         return this;
      }

      public ISessionFactory CreateSessionFactory()
      {
         var configuration = this.CreateConfiguration();

         this.logger.LogDebug("Building NHibernate session factory");
         var sessionFactory = configuration.BuildSessionFactory();

         this.logger.LogDebug("Prepared NHibernate session factory");
         return sessionFactory;
      }

      private Configuration CreateConfiguration()
      {
         this.logger.LogDebug("Creating NHibernate configuration");

         var configuration = new Configuration();

         configuration.LinqToHqlGeneratorsRegistry<AtlasLinqToHqlGeneratorsRegistry>();

         configuration.DataBaseIntegration(c =>
            {
               //// Prevent connection to the database by BuildSessionFactory
               c.KeywordsAutoImport = Hbm2DDLKeyWords.None;
               c.IsolationLevel = IsolationLevel.ReadCommitted;
               c.Timeout = 10;
            });

         foreach (var configurer in this.configurers)
         {
            configurer.Configure(configuration);
         }

         return configuration;
      }
   }
}
