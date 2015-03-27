// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyConvention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.Fluent.Conventions
{
   using System;
   using System.ComponentModel.DataAnnotations;

   using FluentNHibernate.Conventions;
   using FluentNHibernate.Conventions.Instances;

   public class PropertyConvention : IPropertyConvention
   {
      public void Apply(IPropertyInstance instance)
      {
         var propertyType = instance.Property.PropertyType;

         if (propertyType.IsValueType && !(propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
         {
            instance.Not.Nullable();
         }
         else
         {
            if (instance.Property.MemberInfo.IsDefined(typeof(RequiredAttribute), false))
            {
               instance.Not.Nullable();
            }
         }

         // TODO: StringLengthAttribute
      }
   }
}
