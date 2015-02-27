// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowIf.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;

   public static class ThrowIf
   {
      public static void ArgumentIsNull<T>(T arg, string argName) where T : class
      {
         if (arg == null)
         {
            throw new ArgumentNullException(argName);
         }
      }

      public static void ArgumentIsNullOrEmpty(string arg, string argName)
      {
         if (string.IsNullOrEmpty(arg))
         {
            throw new ArgumentNullException(argName);
         }
      }
   }
}
