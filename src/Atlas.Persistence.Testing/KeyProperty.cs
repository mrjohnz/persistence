//-----------------------------------------------------------------------
// <copyright file="KeyProperty.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Linq.Expressions;

   internal class KeyProperty<T_ENTITY, T_PROPERTY> : Property<T_ENTITY, T_PROPERTY>, IKeyProperty<T_ENTITY>
      where T_ENTITY : class
   {
      private readonly Expression<Func<T_ENTITY, bool>> filterExpression;

      internal KeyProperty(IEntityComparer entityComparer, Expression<Func<T_ENTITY, T_PROPERTY>> getterExpression, T_PROPERTY value)
         : base(entityComparer, getterExpression, value)
      {
         ParameterExpression instanceParameter = getterExpression.Parameters[0];
         Expression<Func<T_PROPERTY>> valueExpression = () => this.Value;

         this.filterExpression = Expression.Lambda<Func<T_ENTITY, bool>>(Expression.Equal(getterExpression.Body, valueExpression.Body), instanceParameter);
      }

      Expression<Func<T_ENTITY, bool>> IKeyProperty<T_ENTITY>.FilterExpression
      {
         get { return this.filterExpression; }
      }
   }
}
