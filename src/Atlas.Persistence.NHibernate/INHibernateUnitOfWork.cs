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
   }
}
