// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditModifiedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class AuditModifiedConfiguration : ClassMap<AuditModified>
   {
      public AuditModifiedConfiguration()
      {
         this.Table("AuditModified");
         this.Id(c => c.ID).Column("AuditModifiedID").GeneratedBy.Identity();

         this.Map(c => c.Guid);

         // TODO: Use auto-mapper for this
         this.Map(c => c.ModifiedDateTime).Not.Nullable();
         this.Map(c => c.ModifiedUserGuid).Not.Nullable();
      }
   }
}
