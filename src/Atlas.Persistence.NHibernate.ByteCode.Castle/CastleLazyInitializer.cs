//-----------------------------------------------------------------------
// <copyright file="CastleLazyInitializer.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.ByteCode.Castle
{
   using System;
   using System.Reflection;

   using global::Castle.DynamicProxy;

   using global::NHibernate.Engine;
   using global::NHibernate.Proxy;
   using global::NHibernate.Proxy.Poco;
   using global::NHibernate.Type;

   //// TODO: Need to find credits for this

   [Serializable]
   public class CastleLazyInitializer : BasicLazyInitializer, IInterceptor
   {
      private static readonly MethodInfo ExceptionInternalPreserveStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.NonPublic | BindingFlags.Instance);

      private bool constructed;

      public CastleLazyInitializer(string entityName, Type persistentClass, object id, MethodInfo getIdentifierMethod, MethodInfo setIdentifierMethod, IAbstractComponentType componentIdType, ISessionImplementor session)
         : base(entityName, persistentClass, id, getIdentifierMethod, setIdentifierMethod, componentIdType, session)
      {
      }

      public virtual void Intercept(IInvocation invocation)
      {
         try
         {
            if (this.constructed)
            {
               invocation.ReturnValue = this.Invoke(invocation.Method, invocation.Arguments, invocation.Proxy);

               if (invocation.ReturnValue == AbstractLazyInitializer.InvokeImplementation)
               {
                  invocation.ReturnValue = invocation.Method.Invoke(this.GetImplementation(), invocation.Arguments);
               }
            }
         }
         catch (TargetInvocationException e)
         {
            ExceptionInternalPreserveStackTrace.Invoke(e.InnerException, new object[0]);

            throw e.InnerException;
         }
      }

      public void SetConstructed()
      {
         this.constructed = true;
      }
   }
}