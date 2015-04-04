// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Tests.NHibernateConfiguration
{
   using Atlas.Persistence.TestsBase;

   using NUnit.Framework;

   [TestFixture]
   public class AuditTests : AuditTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory(IAuditConfiguration auditConfiguration, IDateTimeFacility dateTimeFacility, IUserContext userContext)
      {
         return Helper.CreateUnitOfWorkFactory(auditConfiguration: auditConfiguration, dateTimeFacility: dateTimeFacility, userContext: userContext);
      }
   }
}
