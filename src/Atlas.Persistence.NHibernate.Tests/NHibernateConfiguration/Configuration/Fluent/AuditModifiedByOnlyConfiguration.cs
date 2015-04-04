// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedByOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.TestsBase.Entities;

   using FluentNHibernate.Mapping;

   public class AuditModifiedByOnlyConfiguration : ClassMap<AuditModifiedByOnly>
   {
      public AuditModifiedByOnlyConfiguration()
      {
         this.Table("AuditModifiedByOnly");
         this.Id(c => c.ID).Column("AuditModifiedByOnlyID").GeneratedBy.Identity();

         this.Map(c => c.Guid);

         // TODO: Use auto-mapper for this
         this.Map(c => c.ModifiedUserGuid).Not.Nullable();
      }
   }
}
