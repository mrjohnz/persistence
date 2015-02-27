﻿namespace Atlas.Persistence.Testing.Entities
{
   using System;

   public class FooPartitioned : IFoo
   {
      // Guid can be changed without affecting the model

      public virtual long ID { get; protected set; }

      public virtual Guid PartitionGuid { get; protected set; }
      
      public virtual Guid Guid { get; set; }

      public virtual IntEnum IntEnum { get; set; }

      public virtual DateTime? DateTimeValue { get; set; }

      public virtual int? IntValue { get; set; }

      public virtual string StringValue { get; set; }
   }
}
