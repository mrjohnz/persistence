// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyConvention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.ByCode.Conventions
{
   using global::NHibernate.Mapping.ByCode;

   public interface IPropertyConvention : IConvention
   {
      bool Accept(PropertyPath propertyPath);

      void Apply(IPropertyMapper propertyMapper);
   }
}
