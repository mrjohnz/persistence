//-----------------------------------------------------------------------
// <copyright file="EntityQueryableBase.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Implementations
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;

   public abstract class EntityQueryableBase<TEntity, TQueryable> : IEntityQueryable<TEntity>
      where TEntity : class
      where TQueryable : IQueryable<TEntity>
   {
      protected EntityQueryableBase(TQueryable queryable)
      {
         this.Queryable = queryable;
      }

      public Type ElementType
      {
         get { return this.Queryable.ElementType; }
      }

      public Expression Expression
      {
         get { return this.Queryable.Expression; }
      }

      public IQueryProvider Provider
      {
         get { return this.Queryable.Provider; }
      }

      protected TQueryable Queryable { get; private set; }

      public IEntityQueryable<TEntity> Cacheable()
      {
         this.Queryable = this.GetCacheable();

         return this;
      }

      public IEntityQueryable<TEntity> EagerLoad(Expression<Func<TEntity, object>> path)
      {
         this.Queryable = this.GetEagerLoadQueryable(path);

         return this;
      }

      public abstract IEntityQueryable<T> OfType<T>() where T : class, TEntity;

      public IEntityQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter)
      {
         this.Queryable = (TQueryable)this.Queryable.Where(filter);

         return this;
      }

      public IEntityQueryable<TEntity> IsIn<TArgument>(Expression<Func<TEntity, TArgument>> property, TArgument argument)
      {
         return this.IsIn(property, new List<TArgument> { argument });
      }

      // TODO: Need to change this to only send a maximum in one go
      public IEntityQueryable<TEntity> IsIn<TArgument>(Expression<Func<TEntity, TArgument>> property, IEnumerable<TArgument> arguments)
      {
         if (arguments == null)
         {
            throw new ArgumentNullException("arguments");
         }

         // Get the instance from the expression
         var instanceParameter = property.Parameters[0];

         // Ensure that the generated SQL uses a placeholders. Using Expression.Constant translates to a literal.
         var argumentParameters = arguments.Select<TArgument, Expression<Func<TArgument>>>(c => () => c).ToArray();

         // Create the comparison expressions
         var comparisonExpressions = argumentParameters.Select(c => Expression.Equal(property.Body, c.Body)).ToList();

         Expression<Func<TEntity, bool>> filter;

         if (comparisonExpressions.Count != 0)
         {
            // Begin with the first filter
            var aggregateExpression = comparisonExpressions[0];

            // And "Or" the remaining filters. Can't use Expression.Or as this is bitwise and causes problems with NHibernate
            for (var i = 1; i < comparisonExpressions.Count; i++)
            {
               aggregateExpression = Expression.OrElse(aggregateExpression, comparisonExpressions[i]);
            }

            // Create a lambda expression passing in the instance
            filter = Expression.Lambda<Func<TEntity, bool>>(aggregateExpression, instanceParameter);
         }
         else
         {
            // Create an expression that forces the query to return no rows
            filter = c => false;
         }

         // May throw error if lambda method returns an unexpected type
         this.Queryable = (TQueryable)this.Queryable.Where(filter);

         return this;
      }

      public IEnumerator<TEntity> GetEnumerator()
      {
         return this.Queryable.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return this.Queryable.GetEnumerator();
      }

      protected abstract TQueryable GetEagerLoadQueryable(Expression<Func<TEntity, object>> path);

      protected abstract TQueryable GetCacheable();
   }
}
