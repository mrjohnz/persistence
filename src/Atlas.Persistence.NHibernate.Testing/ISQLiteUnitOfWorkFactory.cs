//-----------------------------------------------------------------------
// <copyright file="ISQLiteUnitOfWorkFactory.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Testing
{
   using System;

   using Atlas.Persistence;

   // ReSharper disable once InconsistentNaming
   public interface ISQLiteUnitOfWorkFactory : IUnitOfWorkFactory, IDisposable
   {
      void ExecuteSql(string sql);
   }
}
