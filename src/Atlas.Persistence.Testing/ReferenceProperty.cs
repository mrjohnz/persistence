//-----------------------------------------------------------------------
// <copyright file="ReferenceProperty.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Linq.Expressions;

   using Atlas.Persistence;

   using NUnit.Framework;

   internal class ReferenceProperty<TEntity, TReferenceEntity> : Property<TEntity, TReferenceEntity>
      where TEntity : class
      where TReferenceEntity : class
   {
      internal ReferenceProperty(IEntityComparer entityComparer, Expression<Func<TEntity, TReferenceEntity>> getterExpression, TReferenceEntity value)
         : base(entityComparer, getterExpression, value)
      {
      }

      protected override void AssertAreEqual(TReferenceEntity expected, TReferenceEntity actual)
      {
         if (expected == null && actual != null)
         {
            Assert.Fail("AssertAreEqual failed: actual is '{0}', expected is null", actual.ToString());
         }
         else if (expected != null && actual == null)
         {
            Assert.Fail("AssertAreEqual failed: actual is null, expected is '{0}'", expected.ToString());
         }
         else if (expected != null)
         {
            ThrowIf.Null(this.EntityComparer, "EntityComparer");

            if (!this.EntityComparer.CompareEntity(expected, actual))
            {
               Assert.Fail("AssertAreEqual failed: actual is '{0}', expected is '{1}'", actual.ToString(), expected.ToString());
            }
         }
      }
   }
}
