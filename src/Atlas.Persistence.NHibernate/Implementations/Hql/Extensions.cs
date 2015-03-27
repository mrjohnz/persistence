// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations.Hql
{
   public static class Extensions
   {
      public static T If<T>(bool condition, T @true, T @false)
      {
         if (condition)
         {
            return @true;
         }

         return @false;
      }
   }
}
