// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INHibernateTransaction.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate
{
   using System;

   using global::NHibernate;

   public interface INHibernateTransaction : IDisposable
   {
      ISession Session { get; }
      
      bool TransactionExists { get; }

      void Save();
   }
}
