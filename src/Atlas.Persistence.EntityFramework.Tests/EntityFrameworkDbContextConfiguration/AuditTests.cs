// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using Atlas.Persistence.Testing;

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
