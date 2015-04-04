//-----------------------------------------------------------------------
// <copyright file="ListProperty.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;

   using NUnit.Framework;

   internal class ListProperty<T_ENTITY, T_ITEM, T_LIST> : IProperty<T_ENTITY>
      where T_ENTITY : class
      where T_ITEM : class
      where T_LIST : IEnumerable<T_ITEM>
   {
      private readonly IEntityComparer entityComparer;
      private readonly string name;
      private readonly Func<T_ENTITY, T_LIST> getter;
      private readonly Action<T_ENTITY, T_LIST> setter;
      private readonly Action<T_ENTITY, T_ITEM> addMethod;

      private T_LIST list;

      internal ListProperty(IEntityComparer entityComparer, Expression<Func<T_ENTITY, T_LIST>> getterExpression, T_LIST list)
      {
         this.entityComparer = entityComparer;
         this.name = ((MemberExpression)getterExpression.Body).Member.Name;
         this.list = list;
         this.getter = getterExpression.Compile();

         ParameterExpression instanceParameter = getterExpression.Parameters[0];
         ParameterExpression valueParameter = Expression.Parameter(typeof(T_LIST), "value");

         this.setter = Expression.Lambda<Action<T_ENTITY, T_LIST>>(Expression.Assign(getterExpression.Body, valueParameter), instanceParameter, valueParameter).Compile();
      }

      internal ListProperty(IEntityComparer entityComparer, Expression<Func<T_ENTITY, T_LIST>> getterExpression, Action<T_ENTITY, T_ITEM> addMethod, T_LIST list)
      {
         this.entityComparer = entityComparer;
         this.name = ((MemberExpression)getterExpression.Body).Member.Name;
         this.list = list;
         this.getter = getterExpression.Compile();
         this.addMethod = addMethod;
      }

      public string Name
      {
         get { return this.name; }
      }

      public T_LIST Value
      {
         get { return this.list; }
      }

      bool IProperty<T_ENTITY>.IsReadOnly
      {
         get { return false; }
      }

      public void AssertAreEqual(T_ENTITY entity)
      {
         if (!this.entityComparer.CompareList(this.list, this.getter(entity)))
         {
            Assert.Fail("AssertAreEqual failed: lists don't match");
         }
      }

      void IProperty<T_ENTITY>.PushValue(T_ENTITY entity)
      {
         if (this.setter != null)
         {
            this.setter(entity, this.list);
         }
         else if (this.addMethod != null)
         {
            foreach (T_ITEM item in this.list)
            {
               this.addMethod(entity, item);
            }
         }
      }

      void IProperty<T_ENTITY>.PullValue(T_ENTITY entity)
      {
         this.list = this.getter(entity);
      }
   }
}
