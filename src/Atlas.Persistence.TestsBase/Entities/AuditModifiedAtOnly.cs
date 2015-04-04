// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedAtOnly.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase.Entities
{
   using System;

   public class AuditModifiedAtOnly : IAudit
   {
      //// Guid can be changed without affecting the model

      public virtual long ID { get; protected set; }

      public virtual Guid? Guid { get; set; }

      public virtual DateTime ModifiedDateTime { get; protected set; }
   }
}
