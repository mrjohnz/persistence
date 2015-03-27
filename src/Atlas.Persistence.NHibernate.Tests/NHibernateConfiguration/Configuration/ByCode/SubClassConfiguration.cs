// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClassConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.Testing.Entities;

   using global::NHibernate.Mapping.ByCode.Conformist;

   // TODO: Need to add SubclassMapping classes with discriminators
   // TODO: Need an example of multiple inheritance (eg. ContractRole->Resource->Employee)
   // TODO: How come private set doesn't work with locquacious

   public class SubClassConfiguration : JoinedSubclassMapping<SubClass>
   {
      public SubClassConfiguration()
      {
         this.Key(c => c.Column("BaseClassID"));

         this.Property(c => c.Name, c => c.Length(50));
      }
   }
}
