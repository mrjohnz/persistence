// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFoo.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase.Entities
{
   using System;

   public interface IFoo
   {
      long ID { get; }

      Guid Guid { get; set; }

      IntEnum IntEnum { get; set; }

      DateTime? DateTimeValue { get; set; }

      int? IntValue { get; set; }

      string StringValue { get; set; }
   }
}
