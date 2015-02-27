//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkUnitOfWork.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Implementations
{
   using System;
   using System.Data.Entity.Core;
   using System.Data.Entity.Core.Objects;
   using System.Data.Entity.Infrastructure;
   using System.Globalization;

   using Atlas.Persistence;

   public class EntityFrameworkUnitOfWork : IUnitOfWork
   {
      private readonly Guid unitOfWorkGuid;
      private readonly IEntityFrameworkPersistenceConfiguration configuration;
      private readonly ObjectContext objectContext;
      private readonly IPersistenceLogger persistenceLogger;

      private bool isDisposed;

      private EntityFrameworkUnitOfWork(
         IEntityFrameworkPersistenceConfiguration configuration,
         ObjectContext objectContext,
         IPersistenceLogger persistenceLogger)
      {
         ThrowIf.ArgumentIsNull(objectContext, "objectContext");
         ThrowIf.ArgumentIsNull(persistenceLogger, "persistenceLogger");

         this.unitOfWorkGuid = Guid.NewGuid();
         this.configuration = configuration;
         this.objectContext = objectContext;

         this.objectContext.ContextOptions.LazyLoadingEnabled = true;
         this.objectContext.ContextOptions.ProxyCreationEnabled = true;

         this.persistenceLogger = persistenceLogger;
         this.persistenceLogger.LogDebug("EntityFrameworkUnitOfWork(...) '{0}'", this.unitOfWorkGuid);
      }

      ~EntityFrameworkUnitOfWork()
      {
         this.persistenceLogger.LogWarning("Dispose method of EntityFrameworkUnitOfWork '{0}' has not been called explicitly", this.unitOfWorkGuid);
         this.Dispose(false);
      }

      public void Add<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         this.objectContext.AddObject(this.GetEntitySetName(this.GetRootType(typeof(TEntity))), entity);
      }

      public void Remove<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         this.objectContext.DeleteObject(entity);
      }

      public void Attach<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         this.objectContext.AttachTo(this.GetEntitySetName(this.GetRootType(typeof(TEntity))), entity);
      }

      public void Detach<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         // TODO: How many, if any, related entities should be detached too?

         // Remove the entity from the cache
         this.objectContext.Detach(entity);
      }

      public IEntityQueryable<TEntity> Query<TEntity>()
         where TEntity : class
      {
         var rootType = this.GetRootType(typeof(TEntity));
         ObjectQuery<TEntity> queryable;

         if (rootType == typeof(TEntity))
         {
            queryable = new ObjectQuery<TEntity>(this.GetEntitySetName(typeof(TEntity)), this.objectContext);
         }
         else
         {
            var commandText = string.Format("OFTYPE(({0}),[{1}].[{2}])", this.GetEntitySetName(rootType), typeof(TEntity).Namespace, typeof(TEntity).Name);

            queryable = new ObjectQuery<TEntity>(commandText, this.objectContext);
         }

         return new EntityQueryable<TEntity>(queryable);
      }

      public TEntity Get<TEntity, TKey>(TKey key) where TEntity : class where TKey : struct
      {
         throw new NotSupportedException();
      }

      public TEntity Proxy<TEntity, TKey>(TKey key) where TEntity : class where TKey : struct
      {
         throw new NotSupportedException();
      }

      public void Save()
      {
         this.AssertNotDisposed();

         if (this.objectContext == null)
         {
            return;
         }

         try
         {
            // No need to use SaveOptions.DetectChangesBeforeSave as GetCache will have called DetectChanges
            // this.objectContext.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
            this.objectContext.SaveChanges(SaveOptions.DetectChangesBeforeSave);
         }
         catch (OptimisticConcurrencyException e)
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

      private string GetEntitySetName(Type type)
      {
         return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", this.objectContext.DefaultContainerName, type.Name);
      }

      private Type GetRootType(Type entityType)
      {
         var type = entityType;
         Type rootType = null;

         while (type != null && type != typeof(object))
         {
            // Set rootType as the least deep Type in the heirarchy that is registered
            if (this.configuration.IsEntityRegistered(type))
            {
               rootType = type;
            }

            type = type.BaseType;
         }

         if (rootType == null)
         {
            throw new InvalidOperationException(string.Format("Entity '{0}' has not been registered.", entityType.Name));
         }

         return rootType;
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
            this.persistenceLogger.LogDebug("EntityFrameworkUnitOfWork.Dispose() '{0}'", this.unitOfWorkGuid);

            if (this.objectContext != null)
            {
               this.persistenceLogger.LogDebug("Disposing of ObjectContext");

               this.objectContext.Dispose();
            }
         }

         this.isDisposed = true;
      }

      public class Factory : IUnitOfWorkFactory
      {
         private readonly IEntityFrameworkPersistenceConfiguration configuration;
         private readonly DbCompiledModel model;
         private readonly IPersistenceLogger persistenceLogger;

         public Factory(
            IEntityFrameworkPersistenceConfiguration configuration,
            IPersistenceLogger persistenceLogger)
         {
            ThrowIf.ArgumentIsNull(configuration, "configuration");
            ThrowIf.ArgumentIsNull(persistenceLogger, "persistenceLogger");

            this.configuration = configuration;
            this.model = this.configuration.CreateModel();
            this.persistenceLogger = persistenceLogger;
         }

         public IUnitOfWork Create()
         {
            var connection = this.configuration.CreateConnection();
            var objectContext = this.model.CreateObjectContext<ObjectContext>(connection);

            return new EntityFrameworkUnitOfWork(
               this.configuration,
               objectContext,
               this.persistenceLogger);
         }
      }
   }
}
