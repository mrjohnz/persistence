// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptimisticConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration.Configuration.ByCode
{
   using Atlas.Persistence.NHibernate.UserTypes;
   using Atlas.Persistence.TestsBase.Entities;

   using global::NHibernate.Mapping.ByCode;
   using global::NHibernate.Mapping.ByCode.Conformist;

   public class OptimisticConfiguration : ClassMapping<Optimistic>
   {
      public OptimisticConfiguration()
      {
         this.Id(c => c.ID, c => { c.Column("OptimisticID"); c.Generator(Generators.Identity); });

         this.Property(c => c.StringValue, c => c.Length(50));
         this.Property(c => c.IntValue);
         this.Property(c => c.DateTimeValue);

         this.Version(c => c.Version, c => { c.Column("Version"); c.Type<RowVersionType>(); c.Generated(VersionGeneration.Always); });
      }
   }
}
