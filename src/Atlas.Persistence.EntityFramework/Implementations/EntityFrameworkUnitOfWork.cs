﻿//-----------------------------------------------------------------------
// <copyright file="EntityFrameworkUnitOfWork.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Implementations
{
   using System;
   using System.Collections.Generic;
   using System.Data.Entity;
   using System.Data.Entity.Core;
   using System.Data.Entity.Core.Metadata.Edm;
   using System.Data.Entity.Core.Objects;
   using System.Linq;

   using Atlas.Core.Logging;
   using Atlas.Persistence;

   public class EntityFrameworkUnitOfWork : IUnitOfWork
   {
      private readonly Guid unitOfWorkGuid;
      private readonly ObjectContext objectContext;
      private readonly IInterceptUnitOfWork[] interceptors;
      private readonly ILogger logger;

      private bool isDisposed;

      private EntityFrameworkUnitOfWork(
         ObjectContext objectContext,
         IInterceptUnitOfWork[] interceptors,
         ILogger logger)
      {
         ThrowIf.ArgumentIsNull(objectContext, "objectContext");
         ThrowIf.ArgumentIsNull(interceptors, "interceptors");
         ThrowIf.ArgumentIsNull(logger, "logger");

         this.unitOfWorkGuid = Guid.NewGuid();
         this.objectContext = objectContext;
         this.objectContext.ContextOptions.LazyLoadingEnabled = true;
         this.objectContext.ContextOptions.ProxyCreationEnabled = true;
         this.interceptors = interceptors;

         this.logger = logger;
         this.logger.LogDebug("EntityFrameworkUnitOfWork(...) '{0}'", this.unitOfWorkGuid);
      }

      ~EntityFrameworkUnitOfWork()
      {
         this.logger.LogWarning("Dispose method of EntityFrameworkUnitOfWork '{0}' has not been called explicitly", this.unitOfWorkGuid);
         this.Dispose(false);
      }

      public void Add<TEntity>(TEntity entity)
         where TEntity : class
      {
         ThrowIf.ArgumentIsNull(entity, "entity");

         Type baseType;
         this.objectContext.AddObject(this.GetEntitySetName<TEntity>(out baseType), entity);
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

         Type baseType;
         this.objectContext.AttachTo(this.GetEntitySetName<TEntity>(out baseType), entity);
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
         Type rootType;
         var entitySetName = this.GetEntitySetName<TEntity>(out rootType);

         if (rootType == typeof(TEntity))
         {
            return new EntityQueryable<TEntity>(new ObjectQuery<TEntity>(entitySetName, this.objectContext));
         }

         var commandText = string.Format("OFTYPE(({0}),[{1}].[{2}])", entitySetName, typeof(TEntity).Namespace, typeof(TEntity).Name);

         return new EntityQueryable<TEntity>(new ObjectQuery<TEntity>(commandText, this.objectContext));
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
            if (this.interceptors.Length != 0)
            {
               this.HandleInterceptors();

               // No need to use SaveOptions.DetectChangesBeforeSave as GetCache will have called DetectChanges
               this.objectContext.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
            }
            else
            {
               this.objectContext.SaveChanges(SaveOptions.DetectChangesBeforeSave);
            }
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

      // TODO: Cache these in a dictionary
      private string GetEntitySetName<TEntity>(out Type rootType)
      {
         var type = typeof(TEntity);

         var container = this.objectContext.MetadataWorkspace.GetEntityContainer(this.objectContext.DefaultContainerName, DataSpace.CSpace);
         var baseEntitySets = container.BaseEntitySets.Select(c => new { c.ElementType, c.Name }).ToArray();

         while (type != null && type != typeof(object))
         {
            var entitySet = baseEntitySets.SingleOrDefault(c => c.ElementType.Name == type.Name);

            if (entitySet != null)
            {
               rootType = type;
               return entitySet.Name;
            }

            type = type.BaseType;
         }

         throw new InvalidOperationException(string.Format("Entity '{0}' has not been registered.", typeof(TEntity).Name));
      }

      private void HandleInterceptors()
      {
         const EntityState StateFilter = EntityState.Added | EntityState.Modified | EntityState.Deleted;

         this.objectContext.DetectChanges();

         var entityChanges = new Dictionary<EntityState, object[]>
            {
               { EntityState.Added, new object[0] }, 
               { EntityState.Modified, new object[0] },
               { EntityState.Deleted, new object[0] }
            };

         var changedEntitiesByState = this.objectContext.ObjectStateManager.GetObjectStateEntries(StateFilter)
            .Where(c => !c.IsRelationship)
            .Select(c => new { c.Entity, c.State })
            .GroupBy(c => c.State)
            .ToArray();

         foreach (var changedEntities in changedEntitiesByState)
         {
            entityChanges[changedEntities.Key] = changedEntities.Select(c => c.Entity).ToArray();
         }
         
         foreach (var saveInterceptor in this.interceptors)
         {
            saveInterceptor.Add(entityChanges[EntityState.Added]);
            saveInterceptor.Modify(entityChanges[EntityState.Modified]);
            saveInterceptor.Remove(entityChanges[EntityState.Deleted]);
         }
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
            this.logger.LogDebug("EntityFrameworkUnitOfWork.Dispose() '{0}'", this.unitOfWorkGuid);

            if (this.objectContext != null)
            {
               this.logger.LogDebug("Disposing of ObjectContext");

               this.objectContext.Dispose();
            }
         }

         this.isDisposed = true;
      }

      public class Factory : IUnitOfWorkFactory
      {
         private readonly IEntityFrameworkPersistenceConfiguration configuration;
         private readonly IInterceptUnitOfWork[] interceptors;
         private readonly ILogger logger;

         public Factory(
            IEntityFrameworkPersistenceConfiguration configuration,
            IInterceptUnitOfWork[] interceptors,
            ILogger logger)
         {
            ThrowIf.ArgumentIsNull(configuration, "configuration");
            ThrowIf.ArgumentIsNull(interceptors, "interceptors");
            ThrowIf.ArgumentIsNull(logger, "logger");

            this.configuration = configuration;
            this.interceptors = interceptors;
            this.logger = logger;
         }

         public IUnitOfWork Create()
         {
            var objectContext = this.configuration.CreateObjectContext();

            return new EntityFrameworkUnitOfWork(
               objectContext,
               this.interceptors,
               this.logger);
         }
      }
   }
}
