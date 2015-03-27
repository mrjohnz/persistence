// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XElementConvention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.Fluent.Conventions
{
   using System.Xml.Linq;

   using Atlas.Persistence.NHibernate.UserTypes;

   using FluentNHibernate.Conventions;
   using FluentNHibernate.Conventions.Instances;

   public class XElementConvention : IPropertyConvention
   {
      public void Apply(IPropertyInstance instance)
      {
         var type = instance.Property.PropertyType;

         if (type == typeof(XElement))
         {
            instance.CustomType<XElementUserType>();
         }
      }
   }
}
