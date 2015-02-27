// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAudit.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;

   public interface IAudit
   {
      long ID { get; }

      Guid? Guid { get; set; }
   }
}
