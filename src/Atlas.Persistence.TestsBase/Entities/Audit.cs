// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Audit.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.TestsBase.Entities
{
   using System;

   public class Audit : IAudit
   {
      //// Guid can be changed without affecting the model

      public virtual long ID { get; protected set; }
      
      public virtual Guid? Guid { get; set; }

      public virtual DateTime CreatedDateTime { get; protected set; }

      public virtual Guid CreatedUserGuid { get; protected set; }

      public virtual DateTime ModifiedDateTime { get; protected set; }

      public virtual Guid ModifiedUserGuid { get; protected set; }
   }
}
