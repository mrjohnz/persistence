// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClass.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System.ComponentModel.DataAnnotations;

   public class SubClass : BaseClass
   {
      // Name can be changed without affecting the model

      [StringLength(50)]
      public virtual string Name { get; set; }
   }
}
