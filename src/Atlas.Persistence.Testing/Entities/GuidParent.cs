// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidParent.cs" company="Epworth Consulting Ltd.">
//   � Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;
   using System.ComponentModel.DataAnnotations;

   public class GuidParent
   {
      public virtual Guid Guid { get; protected set; }

      [StringLength(50)]
      public virtual string Name { get; set; }

      public virtual DateTime CreatedDateTime { get; protected set; }
   }
}
