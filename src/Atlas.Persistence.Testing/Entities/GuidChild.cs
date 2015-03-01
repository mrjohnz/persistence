// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidChild.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;
   using System.ComponentModel.DataAnnotations;

   public class GuidChild
   {
      public virtual Guid Guid { get; protected set; }

      [Required]
      public virtual GuidParent GuidParent { get; set; }
   }
}
