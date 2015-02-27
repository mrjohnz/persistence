//-----------------------------------------------------------------------
// <copyright file="IEntityQueryable.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   public interface IEntityQueryable<TEntity> : IQueryable<TEntity>
      where TEntity : class
   {
      IEntityQueryable<TEntity> Cacheable();

      IEntityQueryable<TEntity> EagerLoad(Expression<Func<TEntity, object>> path);

      IEntityQueryable<T> OfType<T>() where T : class, TEntity;

      IEntityQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter);

      IEntityQueryable<TEntity> IsIn<TArgument>(Expression<Func<TEntity, TArgument>> property, TArgument argument);

      IEntityQueryable<TEntity> IsIn<TArgument>(Expression<Func<TEntity, TArgument>> property, IEnumerable<TArgument> arguments);
   }
}
