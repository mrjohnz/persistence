// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBaseClass.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;

   public interface IBaseClass
   {
      long ID { get; }

      Guid Guid { get; set; }

      IntEnum IntEnum { get; set; }
   }
}
