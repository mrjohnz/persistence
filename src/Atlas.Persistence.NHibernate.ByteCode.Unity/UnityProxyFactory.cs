//-----------------------------------------------------------------------
// <copyright file="UnityProxyFactory.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.ByteCode.Unity
{
   using System;

   using Microsoft.Practices.Unity.InterceptionExtension;

   using global::NHibernate;
   using global::NHibernate.Engine;
   using global::NHibernate.Proxy;

   //// TODO: Need to find credits for this

   public class UnityProxyFactory : AbstractProxyFactory
   {
      private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof(UnityProxyFactory));

      public override INHibernateProxy GetProxy(object id, ISessionImplementor session)
      {
         try
         {
            var initializer = new UnityLazyInitializer(this.EntityName, this.PersistentClass, id, this.GetIdentifierMethod, this.SetIdentifierMethod, this.ComponentIdType, session);

            var generatedProxy = Intercept.NewInstanceWithAdditionalInterfaces(
               this.IsClassProxy ? this.PersistentClass : this.Interfaces[0],
               new VirtualMethodInterceptor(),
               new[] { initializer },
               this.Interfaces);

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