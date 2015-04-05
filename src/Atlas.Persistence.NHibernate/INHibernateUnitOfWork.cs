// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INHibernateUnitOfWork.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate
{
   using global::NHibernate;

   public interface INHibernateUnitOfWork : IUnitOfWork
   {
      ISession Session { get; }

      TEntity Get<TEntity, TKey>(TKey key)
         where TEntity : class
         where TKey : struct;

      TEntity Proxy<TEntity, TKey>(TKey key)
         where TEntity : class
         where TKey : struct;

      bool IsProxy<TEntity>(TEntity entity)
         where TEntity : class;
   }
}
