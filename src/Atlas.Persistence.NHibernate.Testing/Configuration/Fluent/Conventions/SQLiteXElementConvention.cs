// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SQLiteXElementConvention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Testing.Configuration.Fluent.Conventions
{
   using System.Xml.Linq;

   using Atlas.Persistence.NHibernate.UserTypes;

   using FluentNHibernate.Conventions;
   using FluentNHibernate.Conventions.Instances;

   // ReSharper disable once InconsistentNaming
   public class SQLiteXElementConvention : IPropertyConvention
   {
      public void Apply(IPropertyInstance instance)
      {
         var type = instance.Property.PropertyType;

         if (type == typeof(XElement))
         {
            instance.CustomType<XElementUserType>();
            instance.CustomSqlType("varchar(4000)");
         }
      }
   }
}
