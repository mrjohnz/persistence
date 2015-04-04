// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SQLiteUnitOfWorkFactory.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Testing
{
   using System;
   using System.Data.SQLite;

   using Atlas.Core.Logging;
   using Atlas.Persistence;
   using Atlas.Persistence.NHibernate;
   using Atlas.Persistence.NHibernate.Implementations;
   using Atlas.Persistence.NHibernate.Testing.Configuration;

   using global::NHibernate;

   // ReSharper disable once InconsistentNaming
   public class SQLiteUnitOfWorkFactory : ISQLiteUnitOfWorkFactory
   {
      private readonly ISessionFactory sessionFactory;
      private readonly IInterceptUnitOfWork[] interceptors;
      private readonly IAuditConfiguration auditConfiguration;
      private readonly IDateTimeFacility dateTimeFacility;
      private readonly IUserContext userContext;
      private readonly ILogger logger;
      private readonly SQLiteConnection connection;

      private bool isDisposed;

      public SQLiteUnitOfWorkFactory(
         INHibernatePersistenceConfiguration configuration,
         IInterceptUnitOfWork[] interceptors,
         IAuditConfiguration auditConfiguration,
         IDateTimeFacility dateTimeFacility,
         IUserContext userContext,
         ILogger logger)
      {
         ThrowIf.ArgumentIsNull(configuration, "configuration");
         ThrowIf.ArgumentIsNull(logger, "logger");

         this.sessionFactory = configuration.CreateSessionFactory();
         this.interceptors = interceptors;
         this.auditConfiguration = auditConfiguration;
         this.dateTimeFacility = dateTimeFacility;
         this.userContext = userContext;
         this.logger = logger;

         // Create an in-memory database connection for the factory. This will be (re)used for each
         // new instance of NHibernateUnitOfWork.
         this.connection = new SQLiteConnection(SQLiteDatabaseConfigurer.InMemoryConnectionString);
         this.connection.Open();

         // Create the schema in the connection based on the NHibernate mappings
         configuration.CreateSchema(this.connection);
      }

      ~SQLiteUnitOfWorkFactory()
      {
         this.Dispose(false);
      }

      public IUnitOfWork Create()
      {
         this.AssertNotDisposed();

         var transaction = new NHibernateTransaction(this.sessionFactory, this.connection, this.interceptors, this.auditConfiguration, this.dateTimeFacility, this.userContext, this.logger);
         
         return new NHibernateUnitOfWork(transaction, this.logger);
      }

      void IDisposable.Dispose()
      {
         this.Dispose(true);
         GC.SuppressFinalize(this);
      }

      public void ExecuteSql(string sql)
      {
         this.AssertNotDisposed();

         using (var command = new SQLiteCommand(sql, this.connection))
         {
            command.ExecuteNonQuery();
         }
      }

      private void AssertNotDisposed()
      {
         if (this.isDisposed)
         {
            throw new ObjectDisposedException(this.GetType().Name);
         }
      }

      private void Dispose(bool isDisposing)
      {
         if (this.isDisposed)
         {
            return;
         }

         if (isDisposing)
         {
            this.connection.Dispose();
         }

         this.isDisposed = true;
      }
   }
}
