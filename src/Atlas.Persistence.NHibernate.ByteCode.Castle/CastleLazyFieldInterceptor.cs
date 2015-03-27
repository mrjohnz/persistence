//-----------------------------------------------------------------------
// <copyright file="CastleLazyFieldInterceptor.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.ByteCode.Castle
{
   using global::Castle.DynamicProxy;

   using global::NHibernate.Intercept;
   using global::NHibernate.Util;

   //// TODO: Need to find credits for this

   public class CastleLazyFieldInterceptor : IFieldInterceptorAccessor, IInterceptor
   {
      public IFieldInterceptor FieldInterceptor { get; set; }

      public void Intercept(IInvocation invocation)
      {
         if (this.FieldInterceptor != null)
         {
            if (ReflectHelper.IsPropertyGet(invocation.Method))
            {
               invocation.Proceed();

               var result = this.FieldInterceptor.Intercept(invocation.InvocationTarget, ReflectHelper.GetPropertyName(invocation.Method), invocation.ReturnValue);

               if (result != AbstractFieldInterceptor.InvokeImplementation)
               {
                  invocation.ReturnValue = result;
               }
            }
            else if (ReflectHelper.IsPropertySet(invocation.Method))
            {
               this.FieldInterceptor.MarkDirty();
               this.FieldInterceptor.Intercept(invocation.InvocationTarget, ReflectHelper.GetPropertyName(invocation.Method), null);

               invocation.Proceed();
            }
            else
            {
               invocation.Proceed();
            }
         }
         else
         {
            invocation.Proceed();
         }
      }
   }
}
