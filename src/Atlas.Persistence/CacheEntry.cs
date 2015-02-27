//-----------------------------------------------------------------------
// <copyright file="CacheEntry.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System;

   [Flags]
   public enum EntryState
   {
      Unchanged = 1,
      Added = 2,
      Modified = 4,
      Removed = 8
   }

   public class CacheEntry
   {
      public CacheEntry(object entity, EntryState state)
      {
         this.Entity = entity;
         this.State = state;
      }

      public object Entity { get; private set; }

      public EntryState State { get; private set; }
   }
}
