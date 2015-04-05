//-----------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;

   // TODO: Need to test for object removed from collection and another readded with identical values. Does persistence layer delete and insert, update or ignore
   public interface IUnitOfWork : IDisposable
   {
      void Add<TEntity>(TEntity entity) where TEntity : class;

      void Remove<TEntity>(TEntity entity) where TEntity : class;

      //// TODO: Consider adding Update/Modify method if possibility entity has changed

      void Attach<TEntity>(TEntity entity) where TEntity : class;

      void Detach<TEntity>(TEntity entity) where TEntity : class;

      IEntityQueryable<TEntity> Query<TEntity>() where TEntity : class;

      void Save();
   }
}
