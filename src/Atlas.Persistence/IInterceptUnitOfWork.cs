// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInterceptUnitOfWork.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   // TODO: Convert from object[] to TEntity[]

   public interface IInterceptUnitOfWork
   {
      void Add(object[] entities);

      void Modify(object[] entities);

      void Remove(object[] entities);
   }
}
