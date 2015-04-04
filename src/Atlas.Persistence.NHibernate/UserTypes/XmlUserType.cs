//-----------------------------------------------------------------------
// <copyright file="XmlUserType.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.UserTypes
{
   using System;
   using System.Data;

   using global::NHibernate.SqlTypes;
   using global::NHibernate.UserTypes;

   public class XmlUserType : IUserType
   {
      public Type ReturnedType
      {
         get { return typeof(string); }
      }

      public SqlType[] SqlTypes
      {
         get { return new SqlType[] { new XmlSqlType() }; }
      }

      public bool IsMutable
      {
         get { return false; }
      }

      public object NullSafeGet(IDataReader rs, string[] names, object owner)
      {
         var index = rs.GetOrdinal(names[0]);

         if (rs.IsDBNull(index))
         {
            return null;
         }

         return rs.GetString(index);
      }

      public void NullSafeSet(IDbCommand cmd, object value, int index)
      {
         var parameter = (IDataParameter)cmd.Parameters[index];

         parameter.Value = value ?? DBNull.Value;
      }

      public object DeepCopy(object value)
      {
         return value;
      }

      public new bool Equals(object x, object y)
      {
         if ((x == null) && (y == null))
         {
            return true;
         }

         var string1 = (string)x;
         var string2 = (string)y;

         return string1 == string2;
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
   }
}
