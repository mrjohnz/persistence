// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrencyException.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;

   public class ConcurrencyException : Exception
   {
      public ConcurrencyException(string message, Exception innerException)
         : base(message, innerException)
      {
      }
   }
}
