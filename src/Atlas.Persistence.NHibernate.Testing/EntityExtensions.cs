// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityExtensions.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Testing
{
   using global::NHibernate.Proxy;

   public static class EntityExtensions
   {
      public static bool IsLoaded(this object entity)
      {
         if (entity == null)
         {
            return false;
         }

         var proxy = entity as INHibernateProxy;

         if (proxy == null)
         {
            return true;
         }

         return !proxy.HibernateLazyInitializer.IsUninitialized;
      }
   }
}
