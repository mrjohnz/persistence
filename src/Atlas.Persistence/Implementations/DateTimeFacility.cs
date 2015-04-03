// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFacility.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Implementations
{
   using System;

   public class DateTimeFacility : IDateTimeFacility
   {
      public DateTime CurrentTime
      {
         get { return DateTime.Now; }
      }
   }
}
