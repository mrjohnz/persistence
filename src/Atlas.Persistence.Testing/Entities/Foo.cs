// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Foo.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;

   public enum IntEnum
   {
      Zero = 0,
      One = 1,
      Two = 2
   }

   public enum CharEnum
   {
      A = 'A',
      B = 'B',
      C = 'C'
   }

   public enum StringEnum
   {
      January,
      February,
      March
   }

   public class Foo : IFoo
   {
      //// Guid can be changed without affecting the model

      public virtual long ID { get; protected set; }

      public virtual Guid Guid { get; set; }

      public virtual IntEnum IntEnum { get; set; }

      public virtual DateTime? DateTimeValue { get; set; }

      public virtual int? IntValue { get; set; }

      public virtual string StringValue { get; set; }
   }
}
