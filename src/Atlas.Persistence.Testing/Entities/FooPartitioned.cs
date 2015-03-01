// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FooPartitioned.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;
   using System.ComponentModel.DataAnnotations;

   public class FooPartitioned : IFoo
   {
      // Guid can be changed without affecting the model

      public virtual long ID { get; protected set; }

      public virtual Guid PartitionGuid { get; protected set; }
      
      public virtual Guid Guid { get; set; }

      public virtual IntEnum IntEnum { get; set; }

      public virtual DateTime? DateTimeValue { get; set; }

      public virtual int? IntValue { get; set; }

      [StringLength(50)]
      public virtual string StringValue { get; set; }
   }
}
