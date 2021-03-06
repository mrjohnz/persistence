﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidParentConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.TestsBase.Entities;

   using FluentNHibernate.Mapping;

   public class GuidParentConfiguration : ClassMap<GuidParent>
   {
      public GuidParentConfiguration()
      {
         this.Table("GuidParent");
         this.Id(c => c.Guid, "GuidParentID");

         this.Map(c => c.Name).Length(50);

         // TODO: Use auto-mapper for this
         this.Map(c => c.CreatedDateTime).Not.Nullable();
      }
   }
}
