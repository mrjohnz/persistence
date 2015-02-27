// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidParent.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;

   public class GuidParent
   {
      public virtual Guid Guid { get; protected set; }

      public virtual string Name { get; set; }

      public virtual DateTime CreatedDateTime { get; protected set; }
   }
}
