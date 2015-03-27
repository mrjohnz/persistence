// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateTransaction.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations
{
   using System;

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
         ILogger logger)
      {
         ThrowIf.ArgumentIsNull(sessionFactory, "sessionFactory");
         ThrowIf.ArgumentIsNull(logger, "logger");

         this.session = sessionFactory.OpenSession();
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
