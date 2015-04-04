// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditTestsBase.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Atlas.Persistence.TestsBase
{
   using System;
   using System.Linq;

   using Atlas.Persistence.Implementations;
   using Atlas.Persistence.Testing;
   using Atlas.Persistence.TestsBase.Entities;

   using NUnit.Framework;

   public abstract class AuditTestsBase
   {
      private AuditConfiguration auditConfiguration;
      private StubDateTimeFacility dateTimeFacility;
      private StubUserContext userContext;
      private IUnitOfWorkFactory unitOfWorkFactory;
      private Guid userGuid;

      [SetUp]
      public void SetupBeforeEachTest()
      {
         this.auditConfiguration = new AuditConfiguration();
         this.dateTimeFacility = new StubDateTimeFacility();
         this.userContext = new StubUserContext();

         this.auditConfiguration
            .Audit<Audit>(c => c.CreatedDateTime, c => c.CreatedUserGuid, c => c.ModifiedDateTime, c => c.ModifiedUserGuid)
            .AuditCreated<AuditCreated>(c => c.CreatedDateTime, c => c.CreatedUserGuid)
            .AuditCreatedDateTime<AuditCreatedAtOnly>(c => c.CreatedDateTime)
            .AuditCreatedUserGuid<AuditCreatedByOnly>(c => c.CreatedUserGuid)
            .AuditModified<AuditModified>(c => c.ModifiedDateTime, c => c.ModifiedUserGuid)
            .AuditModifiedDateTime<AuditModifiedAtOnly>(c => c.ModifiedDateTime)
            .AuditModifiedUserGuid<AuditModifiedByOnly>(c => c.ModifiedUserGuid);

         this.unitOfWorkFactory = this.CreateUnitOfWorkFactory(this.auditConfiguration, this.dateTimeFacility, this.userContext);
         this.userGuid = Guid.Parse("623EE11A-D28F-41b4-A493-74368F648C8D");
         this.userContext.UserGuid = this.userGuid;
      }

      [Test]
      public void CreateAuditCreatedAtOnly()
      {
         this.Create<AuditCreatedAtOnly>(createdAtDateTime: a => a.CreatedDateTime);
      }

      [Test]
      public void CreateAuditCreatedByOnly()
      {
         this.Create<AuditCreatedByOnly>(a => a.CreatedUserGuid);
      }

      [Test]
      public void CreateAuditCreated()
      {
         this.Create<AuditCreated>(a => a.CreatedUserGuid, a => a.CreatedDateTime);
      }

      [Test]
      public void CreateAuditModifiedAtOnly()
      {
         this.Create<AuditModifiedAtOnly>(modifiedAtDateTime: a => a.ModifiedDateTime);
      }

      [Test]
      public void CreateAuditModifiedByOnly()
      {
         this.Create<AuditModifiedByOnly>(a => a.ModifiedUserGuid);
      }

      [Test]
      public void CreateAuditModified()
      {
         this.Create<AuditModified>(a => a.ModifiedUserGuid, a => a.ModifiedDateTime);
      }

      [Test]
      public void CreateAudit()
      {
         this.Create<Audit>(a => a.CreatedUserGuid, a => a.CreatedDateTime, a => a.ModifiedUserGuid, a => a.ModifiedDateTime);
      }

      [Test]
      public void ModifyAuditModifiedAtOnly()
      {
         this.Modify<AuditModifiedAtOnly>(modifiedAtDateTime: a => a.ModifiedDateTime);
      }

      [Test]
      public void ModifyAuditModifiedByOnly()
      {
         this.Modify<AuditModifiedByOnly>(a => a.ModifiedUserGuid);
      }

      [Test]
      public void ModifyAuditModified()
      {
         this.Modify<AuditModified>(a => a.ModifiedUserGuid, a => a.ModifiedDateTime);
      }

      [Test]
      public void ModifyAudit()
      {
         this.Modify<Audit>(a => a.ModifiedUserGuid, a => a.ModifiedDateTime);
      }

      protected abstract IUnitOfWorkFactory CreateUnitOfWorkFactory(IAuditConfiguration auditConfiguration, IDateTimeFacility dateTimeFacility, IUserContext userContext);

      private void Create<T>(
         Func<T, Guid> createdByUserGuid = null, 
         Func<T, DateTime> createdAtDateTime = null, 
         Func<T, Guid> modifiedByUserGuid = null, 
         Func<T, DateTime> modifiedAtDateTime = null)
         where T : class, IAudit, new()
      {
         long auditId;

         this.dateTimeFacility.CurrentTime = new DateTime(2010, 3, 23, 1, 2, 3, 111);

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var audit = new T();

            unitOfWork.Add(audit);
            unitOfWork.Save();

            auditId = audit.ID;
         }

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var audit = unitOfWork.Query<T>().Single(c => c.ID == auditId);

            if (createdByUserGuid != null)
            {
               Assert.AreEqual(this.userGuid, createdByUserGuid(audit));
            }

            if (createdAtDateTime != null)
            {
               AssertDateTime.IsEqual(createdAtDateTime(audit), new DateTime(2010, 3, 23, 1, 2, 3, 111));
            }

            if (modifiedByUserGuid != null)
            {
               Assert.AreEqual(this.userGuid, modifiedByUserGuid(audit));
            }

            if (modifiedAtDateTime != null)
            {
               AssertDateTime.IsEqual(modifiedAtDateTime(audit), new DateTime(2010, 3, 23, 1, 2, 3, 111));
            }
         }
      }

      private void Modify<T>(
         Func<T, Guid> modifiedByUserGuid = null, 
         Func<T, DateTime> modifiedAtDateTime = null)
         where T : class, IAudit, new()
      {
         long auditId;

         this.dateTimeFacility.CurrentTime = new DateTime(2010, 3, 23, 1, 2, 3, 111);

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var audit = new T();

            unitOfWork.Add(audit);
            unitOfWork.Save();

            auditId = audit.ID;
         }

         this.dateTimeFacility.CurrentTime = new DateTime(2011, 4, 19, 4, 5, 6, 222);

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var audit = unitOfWork.Query<T>().Single(c => c.ID == auditId);

            audit.Guid = Guid.NewGuid();

            unitOfWork.Save();
         }

         using (var unitOfWork = this.unitOfWorkFactory.Create())
         {
            var audit = unitOfWork.Query<T>().Single(c => c.ID == auditId);

            if (modifiedByUserGuid != null)
            {
               Assert.AreEqual(this.userGuid, modifiedByUserGuid(audit));
            }

            if (modifiedAtDateTime != null)
            {
               AssertDateTime.IsEqual(modifiedAtDateTime(audit), new DateTime(2011, 4, 19, 4, 5, 6, 222));
            }
         }
      }

      private class StubDateTimeFacility : IDateTimeFacility
      {
         public DateTime CurrentTime { get; set; }
      }

      private class StubUserContext : IUserContext
      {
         public Guid UserGuid { get; set; }
      }
   }
}
