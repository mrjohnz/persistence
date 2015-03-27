// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTime2Convention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.ByCode.Conventions
{
   using System;

   using FluentNHibernate.Conventions.Instances;

   using global::NHibernate;
   using global::NHibernate.Mapping.ByCode;

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

      public bool Accept(PropertyPath propertyPath)
      {
         var memberType = propertyPath.LocalMember.GetPropertyOrFieldType();

         return memberType == typeof(DateTime) || memberType == typeof(DateTime?);
      }

      public void Apply(IPropertyMapper propertyMapper)
      {
         propertyMapper.Type(NHibernateUtil.DateTime2);
      }
   }
}
