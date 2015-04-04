//-----------------------------------------------------------------------
// <copyright file="EntityQueryable.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   using Atlas.Persistence;

   public class EntityQueryable<TEntity> : IEntityQueryable<TEntity>
      where TEntity : class
   {
      private IQueryable<TEntity> queryable;

      public EntityQueryable(IEnumerable<TEntity> list)
      {
         this.queryable = list.AsQueryable();
      }

      private EntityQueryable(IQueryable<TEntity> queryable)
      {
         this.queryable = queryable;
      }

      public Type ElementType
      {
         get { return this.queryable.ElementType; }
      }

      public Expression Expression
      {
         get { return this.queryable.Expression; }
      }

      public IQueryProvider Provider
      {
         get { return this.queryable.Provider; }
      }

      public IEntityQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter)
      {
         this.queryable = this.queryable.Where(filter);

         return this;
      }

      public IEntityQueryable<TEntity> IsIn<TArgument>(Expression<Func<TEntity, TArgument>> property, TArgument argument)
      {
         return this.IsIn(property, new[] { argument });
      }

      public IEntityQueryable<TEntity> IsIn<TArgument>(Expression<Func<TEntity, TArgument>> property, IEnumerable<TArgument> arguments) 
      {
         // This is much easier than the real database EntityQueryable since we don't need to worry about round trips
         var propertyFunc = property.Compile();

         this.queryable = arguments
            .Select(argument => this.queryable.SingleOrDefault(c => EqualityComparer<TArgument>.Default.Equals(propertyFunc(c), argument)))
            .Where(entity => entity != null)
            .ToList()
            .AsQueryable();

         return this;
      }

      public IEntityQueryable<TEntity> EagerLoad(Expression<Func<TEntity, object>> path)
      {
         return this;
      }

      public IEntityQueryable<TEntity> Cacheable()
      {
         return this;
      }

      public IEntityQueryable<T> OfType<T>() where T : class, TEntity
      {
         return new EntityQueryable<T>(this.queryable.OfType<T>());
      }

      public IEnumerator<TEntity> GetEnumerator()
      {
         return this.queryable.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return this.queryable.GetEnumerator();
      }
   }
}
