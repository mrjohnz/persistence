// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionConvention.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration.Fluent.Conventions
{
   using Atlas.Persistence.NHibernate.UserTypes;

   using FluentNHibernate.Conventions;
   using FluentNHibernate.Conventions.Instances;

   public class VersionConvention : IVersionConvention
   {
      public void Apply(IVersionInstance instance)
      {
         instance.CustomType<RowVersionType>();
         instance.CustomSqlType("rowversion");
         instance.Generated.Always();
      }
   }
}
