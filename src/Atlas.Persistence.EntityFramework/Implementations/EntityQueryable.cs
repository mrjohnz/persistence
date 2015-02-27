//-----------------------------------------------------------------------
// <copyright file="EntityQueryable.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Implementations
{
   using System;
   using System.Data.Entity;
   using System.Data.Entity.Core.Objects;
   using System.Linq.Expressions;

   using Atlas.Persistence;
   using Atlas.Persistence.Implementations;

   public class EntityQueryable<TEntity> : EntityQueryableBase<TEntity, ObjectQuery<TEntity>>
      where TEntity : class
   {
      internal EntityQueryable(ObjectQuery<TEntity> queryable)
         : base(queryable)
      {
      }

      public override IEntityQueryable<T> OfType<T>()
      {
         return new EntityQueryable<T>(this.Queryable.OfType<T>());
      }

      protected override ObjectQuery<TEntity> GetEagerLoadQueryable(Expression<Func<TEntity, object>> path)
      {
         // May throw error if lambda method returns an unexpected type
         return (ObjectQuery<TEntity>)this.Queryable.Include(path);
      }

      protected override ObjectQuery<TEntity> GetCacheable()
      {
         return this.Queryable;
      }
   }
}
