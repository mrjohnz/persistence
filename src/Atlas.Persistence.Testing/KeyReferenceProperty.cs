//-----------------------------------------------------------------------
// <copyright file="KeyReferenceProperty.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Linq.Expressions;

   using NUnit.Framework;

   internal class KeyReferenceProperty<T_ENTITY, T_REFERENCE_ENTITY> : KeyProperty<T_ENTITY, T_REFERENCE_ENTITY>
      where T_ENTITY : class
      where T_REFERENCE_ENTITY : class
   {
      internal KeyReferenceProperty(IEntityComparer entityComparer, Expression<Func<T_ENTITY, T_REFERENCE_ENTITY>> getterExpression, T_REFERENCE_ENTITY value)
         : base(entityComparer, getterExpression, value)
      {
      }

      protected override void AssertAreEqual(T_REFERENCE_ENTITY expected, T_REFERENCE_ENTITY actual)
      {
         if ((expected == null) && (actual != null))
         {
            Assert.Fail("AssertAreEqual failed: actual is '{0}', expected is null", actual.ToString());
         }
         else if ((expected != null) && (actual == null))
         {
            Assert.Fail("AssertAreEqual failed: actual is null, expected is '{0}'", expected.ToString());
         }
         else if ((expected != null) && !this.EntityComparer.CompareEntity(expected, actual))
         {
            Assert.Fail("AssertAreEqual failed: actual is '{0}', expected is '{1}'", actual.ToString(), expected.ToString());
         }
      }
   }
}
