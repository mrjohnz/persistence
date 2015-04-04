// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnsiEnumStringType.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.UserTypes
{
   using System;
   using System.Data;
   using System.Linq;

   using global::NHibernate;
   using global::NHibernate.Dialect;
   using global::NHibernate.Engine;
   using global::NHibernate.SqlTypes;
   using global::NHibernate.Type;

   /// <summary>
   /// Ansi equivalent to EnumStringType (for varchar(n) fields)
   /// </summary>
   [Serializable]
   public class AnsiEnumStringType<T> : AbstractEnumType
   {
      public AnsiEnumStringType()
         : base(new AnsiStringSqlType(MaxEnumLength()), typeof(T))
      {
      }

      public override string Name
      {
         get { return "enumstring - " + this.ReturnedClass.Name; }
      }

      public override object Assemble(object cached, ISessionImplementor session, object owner)
      {
         if (cached == null)
         {
            return null;
         }

         return this.GetInstance(cached);
      }

      public override object Disassemble(object value, ISessionImplementor session, object owner)
      {
         if (value != null)
         {
            return this.GetValue(value);
         }

         return null;
      }

      public override object Get(IDataReader rs, int index)
      {
         var code = rs[index];

         if (code != DBNull.Value && code != null)
         {
            return this.GetInstance(code);
         }

         return null;
      }

      public override object Get(IDataReader rs, string name)
      {
         return this.Get(rs, rs.GetOrdinal(name));
      }

      public override string ObjectToSQLString(object value, Dialect dialect)
      {
         return this.GetValue(value).ToString();
      }

      public override void Set(IDbCommand cmd, object value, int index)
      {
         var parameter = (IDataParameter)cmd.Parameters[index];

         if (value == null)
         {
            parameter.Value = DBNull.Value;
         }
         else
         {
            parameter.Value = this.GetValue(value);
         }
      }

      public override string ToString(object value)
      {
         if (value != null)
         {
            return this.GetValue(value).ToString();
         }

         return null;
      }

      private static int MaxEnumLength()
      {
         return Enum.GetNames(typeof(T)).Max(c => c.Length);
      }

      private object GetInstance(object code)
      {
         object instance;

         try
         {
            instance = this.StringToObject(code as string);
         }
         catch (ArgumentException exception)
         {
            throw new HibernateException(string.Format("Can't Parse {0} as {1}", code, this.ReturnedClass.Name), exception);
         }

         return instance;
      }

      private object GetValue(object instance)
      {
         if (instance != null)
         {
            return Enum.Format(this.ReturnedClass, instance, "G");
         }

         return string.Empty;
      }
   }
}
