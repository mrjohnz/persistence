//-----------------------------------------------------------------------
// <copyright file="IdentityKeyProperty.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Linq.Expressions;

   using NUnit.Framework;

   internal class IdentityKeyProperty<T_ENTITY, T_PROPERTY> : KeyProperty<T_ENTITY, T_PROPERTY>, IIdentityKeyProperty<T_ENTITY>
      where T_ENTITY : class
   {
      internal IdentityKeyProperty(IEntityComparer entityComparer, Expression<Func<T_ENTITY, T_PROPERTY>> getterExpression)
         : base(entityComparer, getterExpression, default(T_PROPERTY))
      {
      }

      void IIdentityKeyProperty<T_ENTITY>.AssertNonZeroID()
      {
         Assert.AreNotEqual(this.Value, default(T_PROPERTY));
      }
   }
}
