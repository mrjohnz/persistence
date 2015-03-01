// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Optimistic.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;
   using System.ComponentModel.DataAnnotations;

   public class Optimistic
   {
      public virtual long ID { get; protected set; }

      [StringLength(50)]
      public virtual string StringValue { get; set; }

      public virtual int? IntValue { get; set; }

      public virtual DateTime? DateTimeValue { get; set; }

      public virtual byte[] Version { get; protected set; }
   }
}
