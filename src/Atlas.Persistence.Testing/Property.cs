//-----------------------------------------------------------------------
// <copyright file="Property.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Linq.Expressions;

   using NUnit.Framework;

   internal class Property<T_ENTITY, T_PROPERTY> : IProperty<T_ENTITY>
      where T_ENTITY : class
   {
      protected readonly IEntityComparer EntityComparer;

      private readonly string name;
      private readonly bool isReadOnly;
      private readonly Func<T_ENTITY, T_PROPERTY> getter;
      private readonly Action<T_ENTITY, T_PROPERTY> setter;

      private T_PROPERTY value;

      internal Property(IEntityComparer entityComparer, Expression<Func<T_ENTITY, T_PROPERTY>> getterExpression, T_PROPERTY value)
         : this(entityComparer, getterExpression, value, false)
      {
      }

      internal Property(IEntityComparer entityComparer, Expression<Func<T_ENTITY, T_PROPERTY>> getterExpression, T_PROPERTY value, bool isReadOnly)
      {
         this.EntityComparer = entityComparer;
         this.name = ((MemberExpression)getterExpression.Body).Member.Name;
         this.isReadOnly = isReadOnly;
         this.value = value;
         this.getter = getterExpression.Compile();

         if (!this.isReadOnly)
         {
            ParameterExpression instanceParameter = getterExpression.Parameters[0];
            ParameterExpression valueParameter = Expression.Parameter(typeof(T_PROPERTY), "value");

            this.setter = Expression.Lambda<Action<T_ENTITY, T_PROPERTY>>(Expression.Assign(getterExpression.Body, valueParameter), instanceParameter, valueParameter).Compile();
         }
      }

      public string Name
      {
         get { return this.name; }
      }

      public bool IsReadOnly
      {
         get { return this.isReadOnly; }
      }

      public T_PROPERTY Value
      {
         get { return this.value; }
      }

      public void AssertAreEqual(T_ENTITY entity)
      {
         this.AssertAreEqual(this.value, this.getter(entity));
      }

      void IProperty<T_ENTITY>.PushValue(T_ENTITY entity)
      {
         if (this.isReadOnly)
         {
            throw new InvalidOperationException("Property is read-only");
         }

         this.setter(entity, this.value);
      }

      void IProperty<T_ENTITY>.PullValue(T_ENTITY entity)
      {
         this.value = this.getter(entity);
      }

      protected virtual void AssertAreEqual(T_PROPERTY expected, T_PROPERTY actual)
      {
         if (typeof(T_PROPERTY) == typeof(DateTime?) || typeof(T_PROPERTY) == typeof(DateTime))
         {
            AssertDateTime.IsEqual(expected as DateTime?, actual as DateTime?);
         }
         else
         {
            Assert.AreEqual(expected, actual);
         }
      }

      protected virtual void AssertAreEqual(DateTime expected, DateTime actual)
      {
         AssertDateTime.IsEqual(expected, actual);
      }
   }
}
