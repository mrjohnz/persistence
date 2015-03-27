// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfHqlGenerator.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Implementations.Hql
{
   using System.Collections.ObjectModel;
   using System.Linq.Expressions;
   using System.Reflection;

   using global::NHibernate.Hql.Ast;
   using global::NHibernate.Linq;
   using global::NHibernate.Linq.Functions;
   using global::NHibernate.Linq.Visitors;

   public class IfHqlGenerator : BaseHqlGeneratorForMethod
   {
      public IfHqlGenerator()
      {
         this.SupportedMethods = new[]
            {
               ReflectionHelper.GetMethodDefinition(() => Extensions.If(default(bool), default(object), default(object)))
            };
      }

      public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
      {
         var when = treeBuilder.When(visitor.Visit(arguments[0]).AsExpression(), visitor.Visit(arguments[1]).AsExpression());

         return treeBuilder.Case(new[] { when }, visitor.Visit(arguments[2]).AsExpression());
      }
   }
}
