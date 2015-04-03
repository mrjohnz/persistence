// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditCreatedAtOnly.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing.Entities
{
   using System;

   public class AuditCreatedAtOnly : IAudit
   {
      //// Guid can be changed without affecting the model

      public virtual long ID { get; protected set; }

      public virtual Guid? Guid { get; set; }

      public virtual DateTime CreatedDateTime { get; set; }
   }
}
