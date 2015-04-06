// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateTransaction.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations
{
   using System;
   using System.Data;

   using Atlas.Core.DateTime;
   using Atlas.Core.Logging;
   using Atlas.Persistence.NHibernate;

   using global::NHibernate;

   public class NHibernateTransaction : INHibernateTransaction
   {
      private readonly ISession session;
      private readonly ILogger logger;

      private ITransaction transaction;
      private bool isDisposed;

      public NHibernateTransaction(
         ISessionFactory sessionFactory,
         IDbConnection connection,
         IInterceptUnitOfWork[] interceptors,
         IAuditConfiguration auditConfiguration,
         IDateTime dateTime,
         IUserContext userContext,
         ILogger logger)
      {
         ThrowIf.ArgumentIsNull(sessionFactory, "sessionFactory");
         ThrowIf.ArgumentIsNull(logger, "logger");

         if ((interceptors != null && interceptors.Length != 0) || (auditConfiguration != null && dateTime != null && userContext != null))
         {
            if (connection != null)
            {
               this.session = sessionFactory.OpenSession(connection, new SessionInterceptor(interceptors, auditConfiguration, dateTime, userContext));
            }
            else
            {
               this.session = sessionFactory.OpenSession(new SessionInterceptor(interceptors, auditConfiguration, dateTime, userContext));
            }
         }
         else if (connection != null)
         {
            this.session = sessionFactory.OpenSession(connection);
         }
         else
         {
            this.session = sessionFactory.OpenSession();
         }

         this.session.FlushMode = FlushMode.Commit;
         this.logger = logger;
      }

      ~NHibernateTransaction()
      {
         this.logger.LogWarning("Dispose method of NHibernateTransaction has not been called explicitly");
         this.Dispose(false);
      }

      public ISession Session
      {
         get
         {
            this.AssertNotDisposed();
            this.EnsureTransactionExists();

            return this.session;
         }
      }

      public bool TransactionExists
      {
         get
         {
            this.AssertNotDisposed();

            return this.transaction != null;
         }
      }

      public void Save()
      {
         this.AssertNotDisposed();

         if (this.transaction == null)
         {
            return;
         }

         // The implementation, AdoTransaction performs a Dispose during the Commit
         this.transaction.Commit();
         this.transaction = null;
      }

      void IDisposable.Dispose()
      {
         this.Dispose(true);
         GC.SuppressFinalize(this);
      }

      private void EnsureTransactionExists()
      {
         if (this.transaction != null)
         {
            return;
         }

         this.transaction = this.session.BeginTransaction();
      }

      private void AssertNotDisposed()
      {
         if (this.isDisposed)
         {
            throw new ObjectDisposedException(this.GetType().Name);
         }
      }

      private void Dispose(bool disposing)
      {
         if (this.isDisposed)
         {
            return;
         }

         if (disposing)
         {
            if (this.transaction != null)
            {
               this.transaction.Dispose();
            }

            if (this.session != null)
            {
               this.session.Dispose();
            }
         }

         this.isDisposed = true;
      }
   }
}
