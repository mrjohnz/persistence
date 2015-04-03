// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedAtOnlyConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class AuditModifiedAtOnlyConfiguration : ClassMap<AuditModifiedAtOnly>
   {
      public AuditModifiedAtOnlyConfiguration()
      {
         this.Table("AuditModifiedAtOnly");
         this.Id(c => c.ID).Column("AuditModifiedAtOnlyID").GeneratedBy.Identity();

         this.Map(c => c.Guid);

         // TODO: Use auto-mapper for this
         this.Map(c => c.ModifiedDateTime).Not.Nullable();
      }
   }
}
