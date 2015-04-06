// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using Atlas.Core.DateTime;
   using Atlas.Persistence.TestsBase;

   using NUnit.Framework;

   [TestFixture]
   public class AuditTests : AuditTestsBase
   {
      protected override IUnitOfWorkFactory CreateUnitOfWorkFactory(IAuditConfiguration auditConfiguration, IDateTime dateTime, IUserContext userContext)
      {
         return Helper.CreateUnitOfWorkFactory(auditConfiguration: auditConfiguration, dateTime: dateTime, userContext: userContext);
      }
   }
}
