// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityQueryableTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration
{
   using Atlas.Persistence.Testing;

   public class EntityQueryableTests : EntityQueryableTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
      {
         return Helper.CreateUnitOfWorkFactory();
      }
   }
}
