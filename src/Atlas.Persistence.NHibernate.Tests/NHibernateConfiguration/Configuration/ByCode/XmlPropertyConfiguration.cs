// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlPropertyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.NHibernate.UserTypes;
   using Atlas.Persistence.Testing.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class XmlPropertyConfiguration : ClassMapping<XmlProperty>
   {
      public XmlPropertyConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("XmlPropertyID"); c.Generator(Generators.Identity); });

         this.Property(c => c.Xml, c => c.Type<XElementUserType>());
      }
   }
}
