// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClassPartitionedConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.Testing.Entities;

   using global::NHibernate.Mapping.ByCode.Conformist;

   public class SubClassPartitionedConfiguration : JoinedSubclassMapping<SubClassPartitioned>
   {
      public SubClassPartitionedConfiguration()
      {
         this.Key(c => c.Column("BaseClassID"));

         this.Property(c => c.Name, c => c.Length(50));
      }
   }
}
