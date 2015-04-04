//-----------------------------------------------------------------------
// <copyright file="AnsiEnumCharType.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.UserTypes
{
   using System;
   using System.Data;

   using global::NHibernate;
   using global::NHibernate.Dialect;
   using global::NHibernate.Engine;
   using global::NHibernate.SqlTypes;
   using global::NHibernate.Type;

   /// <summary>
   /// Ansi equivalent to EnumCharType (for char(1) fields)
   /// </summary>
   [Serializable]
   public class AnsiEnumCharType<T> : AbstractEnumType
   {
      public AnsiEnumCharType()
         : base(new AnsiStringFixedLengthSqlType(1), typeof(T))
      {
      }

      public override string Name
      {
         get { return "enumchar - " + this.ReturnedClass.Name; }
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

      public override object FromStringValue(string xml)
      {
         return this.GetInstance(xml);
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
         return string.Format("'{0}'", this.GetValue(value));
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
            parameter.Value = ((char)((int)value)).ToString();
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

      private static char Alternate(char c)
      {
         if (!char.IsUpper(c))
         {
            return char.ToUpper(c);
         }

         return char.ToLower(c);
      }

      private object GetInstance(object code)
      {
         if (code is string)
         {
            return this.GetInstanceFromString((string)code);
         }

         if (!(code is char))
         {
            throw new HibernateException(string.Format("Can't Parse {0} as {1}", code, this.ReturnedClass.Name));
         }

         return this.GetInstanceFromChar((char)code);
      }

      private object GetInstanceFromChar(char c)
      {
         var instance = Enum.ToObject(this.ReturnedClass, c);

         if (!Enum.IsDefined(this.ReturnedClass, instance))
         {
            instance = Enum.ToObject(this.ReturnedClass, Alternate(c));

            if (!Enum.IsDefined(this.ReturnedClass, instance))
            {
               throw new HibernateException(string.Format("Can't Parse {0} as {1}", c, this.ReturnedClass.Name));
            }
         }

         return instance;
      }

      private object GetInstanceFromString(string s)
      {
         if (s.Length == 0)
         {
            throw new HibernateException(string.Format("Can't Parse empty string as {0}", this.ReturnedClass.Name));
         }
         
         if (s.Length == 1)
         {
            return this.GetInstanceFromChar(s[0]);
         }

         object value;

         try
         {
            value = Enum.Parse(this.ReturnedClass, s, false);
         }
         catch (ArgumentException)
         {
            try
            {
               value = Enum.Parse(this.ReturnedClass, s, true);
            }
            catch (ArgumentException ae)
            {
               throw new HibernateException(string.Format("Can't Parse {0} as {1}", s, this.ReturnedClass.Name), ae);
            }
         }

         return value;
      }

      private object GetValue(object instance)
      {
         if (instance == null)
         {
            return null;
         }

         return (char)instance;
      }
   }
}
