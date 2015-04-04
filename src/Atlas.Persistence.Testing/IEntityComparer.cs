//-----------------------------------------------------------------------
// <copyright file="IEntityComparer.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System.Collections.Generic;

   public interface IEntityComparer
   {
      bool CompareEntity<TEntity>(TEntity arg1, TEntity arg2) where TEntity : class;

      bool CompareList<TEntity>(IEnumerable<TEntity> arg1, IEnumerable<TEntity> arg2) where TEntity : class;
   }
}
