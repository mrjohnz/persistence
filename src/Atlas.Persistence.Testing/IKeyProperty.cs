//-----------------------------------------------------------------------
// <copyright file="IKeyProperty.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Linq.Expressions;

   public interface IKeyProperty<T_ENTITY> : IProperty<T_ENTITY>
      where T_ENTITY : class
   {
      Expression<Func<T_ENTITY, bool>> FilterExpression { get; }
   }
}
