// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDateTimeFacility.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;

   public interface IDateTimeFacility
   {
      DateTime CurrentTime { get; }
   }
}
