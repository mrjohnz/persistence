//-----------------------------------------------------------------------
// <copyright file="XElementUserType.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.UserTypes
{
   using System;
   using System.Data;
   using System.Xml.Linq;

   using global::NHibernate.SqlTypes;
   using global::NHibernate.UserTypes;

   public class XElementUserType : IUserType
   {
      public Type ReturnedType
      {
         get { return typeof(XElement); }
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

         return XElement.Parse(rs.GetString(index));
      }

      public void NullSafeSet(IDbCommand cmd, object value, int index)
      {
         var parameter = (IDataParameter)cmd.Parameters[index];

         if (value == null)
         {
            parameter.Value = DBNull.Value;
         }
         else
         {
            parameter.Value = value.ToString();
         }
      }

      public object DeepCopy(object value)
      {
         return value;
      }

      public new bool Equals(object x, object y)
      {
         if (x == null && y == null)
         {
            return true;
         }

         if (x == null || y == null)
         {
            return false;
         }

         var xml1 = (XElement)x;
         var xml2 = (XElement)y;

         return xml1.Equals(xml2);
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
