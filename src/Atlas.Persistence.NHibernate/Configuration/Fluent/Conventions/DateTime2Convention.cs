// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTime2Convention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.Fluent.Conventions
{
   using System;

   using FluentNHibernate.Conventions;
   using FluentNHibernate.Conventions.Instances;

   public class DateTime2Convention : IPropertyConvention
   {
      public void Apply(IPropertyInstance instance)
      {
         var type = instance.Property.PropertyType;

         if (type == typeof(DateTime) || type == typeof(DateTime?))
         {
            instance.CustomType("timestamp");
            instance.CustomSqlType("datetime2");
         }
      }
   }
}
