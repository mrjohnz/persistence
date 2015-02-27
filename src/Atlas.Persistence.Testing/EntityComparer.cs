//-----------------------------------------------------------------------
// <copyright file="EntityComparer.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Collections.Generic;

   public class EntityComparer
   {
      private readonly Dictionary<Type, Delegate> comparers = new Dictionary<Type, Delegate>();

      public EntityComparer()
      {
         this.Add<string>(Compare);
         this.Add<Guid>(Compare);
         this.Add<DateTime>(Compare);
      }

      public void Add<T>(Func<T, T, bool> comparer)
      {
         if (this.comparers.ContainsKey(typeof(T)))
         {
            throw new InvalidOperationException(string.Format("Comparer for '{0}' is already registered.", typeof(T).Name));
         }

         this.comparers.Add(typeof(T), comparer);
      }

      public void Remove<T>()
      {
         if (!this.comparers.ContainsKey(typeof(T)))
         {
            throw new InvalidOperationException(string.Format("Comparer for '{0}' is not registered.", typeof(T).Name));
         }

         this.comparers.Remove(typeof(T));
      }

      public bool CompareEntity<T>(T arg1, T arg2)
         where T : class
      {
         Delegate comparer;

         if (!this.comparers.TryGetValue(typeof(T), out comparer))
         {
            throw new InvalidOperationException(string.Format("Comparer for '{0}' is not registered.", typeof(T).Name));
         }

         var typedComparer = comparer as Func<T, T, bool>;

         if (typedComparer == null)
         {
            throw new InvalidOperationException(string.Format("Comparer for '{0}' could not be determined.", typeof(T).Name));
         }

         return typedComparer(arg1, arg2);
      }

      public bool CompareList<T>(IEnumerable<T> arg1, IEnumerable<T> arg2)
         where T : class
      {
         Delegate comparer;

         if (!this.comparers.TryGetValue(typeof(T), out comparer))
         {
            throw new InvalidOperationException(string.Format("Comparer for '{0}' is not registered.", typeof(T).Name));
         }

         var typedComparer = comparer as Func<T, T, bool>;

         if (typedComparer == null)
         {
            throw new InvalidOperationException(string.Format("Comparer for '{0}' could not be determined.", typeof(T).Name));
         }

         var list1 = new List<T>(arg1);
         var list2 = new List<T>(arg2);

         if (list1.Count != list2.Count)
         {
            return false;
         }

         for (var i = 0; i < list1.Count; i++)
         {
            if (!typedComparer(list1[i], list2[i]))
            {
               return false;
            }
         }

         return true;
      }

      protected static bool Compare(string expected, string actual)
      {
         // TODO: Improve string compare
         return expected == actual;
      }

      protected static bool Compare(Guid expected, Guid actual)
      {
         return expected == actual;
      }

      protected static bool Compare(DateTime expected, DateTime actual)
      {
         return expected == actual;
      }
   }
}
