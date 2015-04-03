// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   public class AuditConfiguration : ClassMap<Audit>
   {
      public AuditConfiguration()
      {
         this.Table("Audit");
         this.Id(c => c.ID, "AuditID").GeneratedBy.Identity();

         this.Map(c => c.Guid);

         // TODO: Use auto-mapper for this
         this.Map(c => c.CreatedDateTime).Not.Nullable();
         this.Map(c => c.CreatedUserGuid).Not.Nullable();
         this.Map(c => c.ModifiedDateTime).Not.Nullable();
         this.Map(c => c.ModifiedUserGuid).Not.Nullable();
      }
   }
}
