//-----------------------------------------------------------------------
// <copyright file="UnityProxyFactoryFactory.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.ByteCode.Unity
{
   using global::NHibernate.Bytecode;
   using global::NHibernate.Proxy;

   //// TODO: Need to find credits for this

   public class UnityProxyFactoryFactory : IProxyFactoryFactory
   {
      public IProxyValidator ProxyValidator
      {
         get { return new DynProxyTypeValidator(); }
      }

      public IProxyFactory BuildProxyFactory()
      {
         return new UnityProxyFactory();
      }

      public bool IsInstrumented(System.Type entityClass)
      {
         return false;
      }

      public bool IsProxy(object entity)
      {
         return entity is INHibernateProxy;
      }
   }
}