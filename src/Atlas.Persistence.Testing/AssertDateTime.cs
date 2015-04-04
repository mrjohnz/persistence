// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssertDateTime.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;

   using NUnit.Framework;

   public static class AssertDateTime
   {
      public static void IsEqual(DateTime? expected, DateTime? actual)
      {
         if ((expected == null) && (actual != null))
         {
            Assert.Fail("AssertDateEqual failed: actual is '{0}', expected is null", actual);
         }
         else if ((expected != null) && (actual == null))
         {
            Assert.Fail("AssertDateEqual failed: actual is null, expected is '{0}'", expected);
         }

         if (expected != null)
         {
            TimeSpan difference = expected.Value.Subtract(actual.Value);

            if (Math.Abs(difference.TotalSeconds) >= 1)
            {
               Assert.Fail("AssertDateEqual failed: actual is '{0}', expected is '{1}'", actual, expected);
            }
         }
      }
   }
}
