//-----------------------------------------------------------------------
// <copyright file="CastleProxyFactory.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.ByteCode.Castle
{
   using System;

   using global::Castle.DynamicProxy;

   using global::NHibernate;
   using global::NHibernate.Engine;
   using global::NHibernate.Proxy;

   //// TODO: Need to find credits for this

   public class CastleProxyFactory : AbstractProxyFactory
   {
      private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof(CastleProxyFactory));
      private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

      public override INHibernateProxy GetProxy(object id, ISessionImplementor session)
      {
         try
         {
            var initializer = new CastleLazyInitializer(this.EntityName, this.PersistentClass, id, this.GetIdentifierMethod, this.SetIdentifierMethod, this.ComponentIdType, session);

            object generatedProxy;

            if (this.IsClassProxy)
            {
               generatedProxy = ProxyGenerator.CreateClassProxy(this.PersistentClass, this.Interfaces, initializer);
            }
            else
            {
               generatedProxy = ProxyGenerator.CreateInterfaceProxyWithoutTarget(this.Interfaces[0], this.Interfaces, initializer);
            }

            initializer.SetConstructed();

            return (INHibernateProxy)generatedProxy;
         }
         catch (Exception e)
         {
            Log.Error("Creating a proxy instance failed", e);

            throw new HibernateException("Creating a proxy instance failed", e);
         }
      }
   }
}