// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClassPartitioned.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase.Entities
{
   using System.ComponentModel.DataAnnotations;

   public class SubClassPartitioned : BaseClassPartitioned
   {
      // Name can be changed without affecting the model

      [StringLength(50)]
      public virtual string Name { get; set; }
   }
}
