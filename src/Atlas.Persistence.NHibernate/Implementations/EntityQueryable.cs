//-----------------------------------------------------------------------
// <copyright file="EntityQueryable.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations
{
   using System;
   using System.Linq;
   using System.Linq.Expressions;

   using Atlas.Persistence;
   using Atlas.Persistence.Implementations;

   using global::NHibernate.Linq;

   public class EntityQueryable<TEntity> : EntityQueryableBase<TEntity, IQueryable<TEntity>>
      where TEntity : class
   {
      internal EntityQueryable(IQueryable<TEntity> queryable)
         : base(queryable)
      {
      }

      public override IEntityQueryable<T> OfType<T>()
      {
         // TODO: This doesn't produce efficient SQL. All subclasses are left-joined and the "type" is filtered in the where clause.
         return new EntityQueryable<T>(this.Queryable.OfType<T>());
      }

      protected override IQueryable<TEntity> GetEagerLoadQueryable(Expression<Func<TEntity, object>> path)
      {
         return this.Queryable.Fetch(path);
      }

      protected override IQueryable<TEntity> GetCacheable()
      {
         return this.Queryable.Cacheable();
      }
   }
}
