// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IgnoreVersionConvention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Testing.Configuration.Fluent.Conventions
{
   using FluentNHibernate.Conventions;
   using FluentNHibernate.Conventions.Instances;

   public class IgnoreVersionConvention : IVersionConvention
   {
      public void Apply(IVersionInstance instance)
      {
         instance.Nullable();
      }
   }
}
