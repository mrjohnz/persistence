//-----------------------------------------------------------------------
// <copyright file="NHibernateUnitOfWork.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations
{
   using System;

   using Atlas.Core.DateTime;
   using Atlas.Core.Logging;
   using Atlas.Persistence;

   using global::NHibernate;
   using global::NHibernate.Linq;
   using global::NHibernate.Proxy;

   public class NHibernateUnitOfWork : INHibernateUnitOfWork
   {
      private readonly Guid unitOfWorkGuid;
      private readonly INHibernateTransaction transaction;
      private readonly ILogger logger;

      private bool isDisposed;

      public NHibernateUnitOfWork(
         INHibernateTransaction transaction,
         ILogger logger)
      {
         ThrowIf.ArgumentIsNull(transaction, "transaction");
         ThrowIf.ArgumentIsNull(logger, "logger");

         this.unitOfWorkGuid = Guid.NewGuid();
         this.transaction = transaction;

         this.logger = logger;
         this.logger.LogDebug("NHibernateUnitOfWork(...) '{0}'", this.unitOfWorkGuid);
      }

      ~NHibernateUnitOfWork()
      {
         this.logger.LogWarning("Dispose method of NHibernateUnitOfWork '{0}' has not been called explicitly", this.unitOfWorkGuid);
         this.Dispose(false);
      }

      public ISession Session
      {
         get { return this.transaction.Session; }
      }

      public void Add<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         // For any transient entity errors, ensure Cascade.SaveUpdate() has been specified in respective References mappings
         this.transaction.Session.Save(entity);
      }

      public void Remove<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         this.transaction.Session.Delete(entity);
      }

      public void Attach<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         // Attach the transient entity to the session
         this.transaction.Session.Update(entity);
      }

      public void Detach<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         // Remove the entity from the cache
         this.transaction.Session.Evict(entity);
      }

      public IEntityQueryable<TEntity> Query<TEntity>()
         where TEntity : class
      {
         return new EntityQueryable<TEntity>(this.transaction.Session.Query<TEntity>());
      }

      public TEntity Get<TEntity, TKey>(TKey key)
         where TEntity : class
         where TKey : struct
      {
         this.AssertNotDisposed();

         return this.transaction.Session.Get<TEntity>(key);
      }

      public TEntity Proxy<TEntity, TKey>(TKey key)
         where TEntity : class
         where TKey : struct
      {
         this.AssertNotDisposed();

         return this.transaction.Session.Load<TEntity>(key);
      }

      public void Save()
      {
         this.AssertNotDisposed();

         if (this.transaction == null || !this.transaction.TransactionExists)
         {
            return;
         }

         try
         {
            this.transaction.Save();
         }
         catch (StaleObjectStateException e)
         {
            this.Dispose(true);

            throw new ConcurrencyException("Data has been changed by another transaction", e);
         }
         catch
         {
            this.Dispose(true);

            throw;
         }
      }

      void IDisposable.Dispose()
      {
         this.Dispose(true);
         GC.SuppressFinalize(this);
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
            this.logger.LogDebug("NHibernateUnitOfWork.Dispose() '{0}'", this.unitOfWorkGuid);

            if (this.transaction != null)
            {
               this.logger.LogDebug("Disposing of INHibernateTransaction");

               this.transaction.Dispose();
            }
         }

         this.isDisposed = true;
      }

      // TODO: Needs disposable for this.sessionFactory
      public class Factory : IUnitOfWorkFactory
      {
         private readonly ISessionFactory sessionFactory;
         private readonly IInterceptUnitOfWork[] interceptors;
         private readonly IAuditConfiguration auditConfiguration;
         private readonly IDateTime dateTime;
         private readonly IUserContext userContext;
         private readonly ILogger logger;

         public Factory(
            INHibernatePersistenceConfiguration configuration,
            IInterceptUnitOfWork[] interceptors,
            IAuditConfiguration auditConfiguration,
            IDateTime dateTime,
            IUserContext userContext,
            ILogger logger)
         {
            ThrowIf.ArgumentIsNull(configuration, "configuration");
            ThrowIf.ArgumentIsNull(logger, "logger");

            this.sessionFactory = configuration.CreateSessionFactory();
            this.interceptors = interceptors;
            this.auditConfiguration = auditConfiguration;
            this.dateTime = dateTime;
            this.userContext = userContext;
            this.logger = logger;
         }
         
         public IUnitOfWork Create()
         {
            var transaction = new NHibernateTransaction(this.sessionFactory, null, this.interceptors, this.auditConfiguration, this.dateTime, this.userContext, this.logger);

            return new NHibernateUnitOfWork(transaction, this.logger);
         }
      }
   }
}
