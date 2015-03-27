// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidChildConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class GuidChildConfiguration : ClassMap<GuidChild>
   {
      public GuidChildConfiguration()
      {
         this.Table("GuidChild");
         this.Id(c => c.Guid, "GuidChildID").GeneratedBy.GuidComb();

         this.References(c => c.GuidParent, "GuidParentID").Not.Nullable();
      }
   }
}
