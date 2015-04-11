// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SQLiteUnitOfWorkFactory.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Testing
{
   using System;
   using System.Data.SQLite;

   using Atlas.Core.DateTime;
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
      private readonly IDateTime dateTime;
      private readonly IUserContext userContext;
      private readonly ILogger logger;
      private readonly SQLiteConnection connection;

      private bool isDisposed;

      public SQLiteUnitOfWorkFactory(
         INHibernatePersistenceConfiguration configuration,
         ISessionFactory sessionFactory,
         IInterceptUnitOfWork[] interceptors,
         IAuditConfiguration auditConfiguration,
         IDateTime dateTime,
         IUserContext userContext,
         ILogger logger)
      {
         ThrowIf.ArgumentIsNull(configuration, "configuration");
         ThrowIf.ArgumentIsNull(sessionFactory, "sessionFactory");
         ThrowIf.ArgumentIsNull(logger, "logger");

         this.sessionFactory = sessionFactory;
         this.interceptors = interceptors;
         this.auditConfiguration = auditConfiguration;
         this.dateTime = dateTime;
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

         var transaction = new NHibernateTransaction(this.sessionFactory, this.connection, this.interceptors, this.auditConfiguration, this.dateTime, this.userContext, this.logger);
         
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
