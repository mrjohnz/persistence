//-----------------------------------------------------------------------
// <copyright file="CastleProxyFactoryFactory.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.ByteCode.Castle
{
   using System;

   using global::NHibernate.Bytecode;
   using global::NHibernate.Proxy;

   //// TODO: Need to find credits for this

   public class CastleProxyFactoryFactory : IProxyFactoryFactory
   {
      public IProxyValidator ProxyValidator
      {
         get { return new DynProxyTypeValidator(); }
      }

      public IProxyFactory BuildProxyFactory()
      {
         return new CastleProxyFactory();
      }

      public bool IsInstrumented(Type entityClass)
      {
         return true;
      }

      public bool IsProxy(object entity)
      {
         return entity is INHibernateProxy;
      }
   }
}