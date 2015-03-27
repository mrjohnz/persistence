// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AtlasLinqToHqlGeneratorsRegistry.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations.Hql
{
   using global::NHibernate.Linq.Functions;

   public class AtlasLinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
   {
      public AtlasLinqToHqlGeneratorsRegistry()
      {
         this.Merge(new IfHqlGenerator());
      }
   }
}
