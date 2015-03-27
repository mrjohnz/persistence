// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubClassConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.Fluent
{
   using Atlas.Persistence.Testing.Entities;

   using FluentNHibernate.Mapping;

   // TODO: Need to add SubclassMapping classes with discriminators
   // TODO: Need an example of multiple inheritance (eg. ContractRole->Resource->Employee)
   // TODO: How come private set doesn't work with locquacious

   public class SubClassConfiguration : SubclassMap<SubClass>
   {
      public SubClassConfiguration()
      {
         this.Table("SubClass");
         this.KeyColumn("BaseClassID");

         this.Map(c => c.Name).Length(50);
      }
   }
}
