// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlPropertyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class XmlPropertyConfiguration : ClassMap<XmlProperty>
   {
      public XmlPropertyConfiguration()
      {
         this.Table("XmlProperty");
         this.Id(c => c.ID, "XmlPropertyID").GeneratedBy.Identity();

         this.Map(c => c.Xml);
      }
   }
}
