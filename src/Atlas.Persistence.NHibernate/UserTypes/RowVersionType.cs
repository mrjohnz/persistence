// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RowVersionType.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.UserTypes
{
   using System;
   using System.Data;

   using global::NHibernate;
   using global::NHibernate.Engine;
   using global::NHibernate.SqlTypes;
   using global::NHibernate.UserTypes;

   public class RowVersionType : IUserVersionType
   {
      public Type ReturnedType
      {
         get { return typeof(byte[]); }
      }

      public SqlType[] SqlTypes
      {
         get { return new[] { new SqlType(DbType.Binary, 8) }; }
      }

      public bool IsMutable
      {
         get { return false; }
      }

      public object NullSafeGet(IDataReader rs, string[] names, object owner)
      {
         return rs.GetValue(rs.GetOrdinal(names[0]));
      }

      public void NullSafeSet(IDbCommand cmd, object value, int index)
      {
         NHibernateUtil.Binary.NullSafeSet(cmd, value, index);
      }

      public object DeepCopy(object value)
      {
         return value;
      }

      public new bool Equals(object x, object y)
      {
         return x == y;
      }

      public int GetHashCode(object x)
      {
         return x.GetHashCode();
      }

      public object Assemble(object cached, object owner)
      {
         throw new NotSupportedException();
      }

      public object Disassemble(object value)
      {
         throw new NotSupportedException();
      }

      public object Replace(object original, object target, object owner)
      {
         throw new NotSupportedException();
      }

      public object Next(object current, ISessionImplementor session)
      {
         return current;
      }

      public object Seed(ISessionImplementor session)
      {
         return new byte[8];
      }

      public int Compare(object x, object y)
      {
         var xbytes = (byte[])x;
         var ybytes = (byte[])y;

         return CompareValues(xbytes, ybytes);
      }

      private static int CompareValues(byte[] x, byte[] y)
      {
         if (x.Length < y.Length)
         {
            return -1;
         }

         if (x.Length > y.Length)
         {
            return 1;
         }

         for (var i = 0; i < x.Length; i++)
         {
            if (x[i] < y[i])
            {
               return -1;
            }

            if (x[i] > y[i])
            {
               return 1;
            }
         }

         return 0;
      }
   }
}
