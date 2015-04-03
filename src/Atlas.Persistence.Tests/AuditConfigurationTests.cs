// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditConfigurationTests.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Tests
{
   using System;

   using Atlas.Persistence.Implementations;

   using NUnit.Framework;

   public class AuditConfigurationTests
   {
      [Test]
      public void CreatedDateTimePropertyNameReturnsNullIfNoRegistration()
      {
         var componentUnderTest = new AuditConfiguration();

         Assert.That(componentUnderTest.CreatedDateTimePropertyName(typeof(TestEntity)), Is.Null);
      }

      [Test]
      public void ModifiedDateTimePropertyNameReturnsNullIfNoRegistration()
      {
         var componentUnderTest = new AuditConfiguration();

         Assert.That(componentUnderTest.ModifiedDateTimePropertyName(typeof(TestEntity)), Is.Null);
      }

      [Test]
      public void CreatedUserGuidPropertyNameReturnsNullIfNoRegistration()
      {
         var componentUnderTest = new AuditConfiguration();

         Assert.That(componentUnderTest.CreatedUserGuidPropertyName(typeof(TestEntity)), Is.Null);
      }

      [Test]
      public void ModifiedUserGuidPropertyNameReturnsNullIfNoRegistration()
      {
         var componentUnderTest = new AuditConfiguration();

         Assert.That(componentUnderTest.ModifiedUserGuidPropertyName(typeof(TestEntity)), Is.Null);
      }

      [Test]
      public void AuditCreatedDateTimeSetsCreatedDateTimePropertyName()
      {
         var componentUnderTest = new AuditConfiguration();

         componentUnderTest.AuditCreatedDateTime<TestEntity>(c => c.DateTime);

         Assert.That(componentUnderTest.CreatedDateTimePropertyName(typeof(TestEntity)), Is.EqualTo("DateTime"));
      }

      [Test]
      public void AuditModifiedDateTimeSetsCreatedDateTimePropertyName()
      {
         var componentUnderTest = new AuditConfiguration();

         componentUnderTest.AuditModifiedDateTime<TestEntity>(c => c.AnotherDateTime);

         Assert.That(componentUnderTest.ModifiedDateTimePropertyName(typeof(TestEntity)), Is.EqualTo("AnotherDateTime"));
      }

      [Test]
      public void AuditCreatedUserGuidSetsCreatedUserGuidPropertyName()
      {
         var componentUnderTest = new AuditConfiguration();

         componentUnderTest.AuditCreatedUserGuid<TestEntity>(c => c.SomeGuid);

         Assert.That(componentUnderTest.CreatedUserGuidPropertyName(typeof(TestEntity)), Is.EqualTo("SomeGuid"));
      }

      [Test]
      public void AuditModifiedUserGuidSetsModifiedUserGuidPropertyName()
      {
         var componentUnderTest = new AuditConfiguration();

         componentUnderTest.AuditModifiedUserGuid<TestEntity>(c => c.AnotherGuid);

         Assert.That(componentUnderTest.ModifiedUserGuidPropertyName(typeof(TestEntity)), Is.EqualTo("AnotherGuid"));
      }

      [Test]
      public void AuditCreatedDateTimeSetsCreatedDateTimeProperty()
      {
         var componentUnderTest = new AuditConfiguration();
         var entity = new TestEntity();
         var dateTime = new DateTime(2015, 4, 3, 20, 19, 24);

         componentUnderTest.AuditCreatedDateTime<TestEntity>(c => c.DateTime);
         componentUnderTest.AuditCreatedDateTime(typeof(TestEntity), new object[] { entity }, dateTime);

         Assert.That(entity.DateTime, Is.EqualTo(dateTime));
      }

      [Test]
      public void AuditModifiedDateTimeSetsCreatedDateTimeProperty()
      {
         var componentUnderTest = new AuditConfiguration();
         var entity = new TestEntity();
         var dateTime = new DateTime(2015, 4, 3, 20, 19, 24);

         componentUnderTest.AuditModifiedDateTime<TestEntity>(c => c.AnotherDateTime);
         componentUnderTest.AuditModifiedDateTime(typeof(TestEntity), new object[] { entity }, dateTime);

         Assert.That(entity.AnotherDateTime, Is.EqualTo(dateTime));
      }

      [Test]
      public void AuditCreatedUserGuidSetsCreatedUserGuidProperty()
      {
         var componentUnderTest = new AuditConfiguration();
         var entity = new TestEntity();
         var userGuid = Guid.NewGuid();

         componentUnderTest.AuditCreatedUserGuid<TestEntity>(c => c.SomeGuid);
         componentUnderTest.AuditCreatedUserGuid(typeof(TestEntity), new object[] { entity }, userGuid);

         Assert.That(entity.SomeGuid, Is.EqualTo(userGuid));
      }

      [Test]
      public void AuditModifiedUserGuidSetsModifiedUserGuidProperty()
      {
         var componentUnderTest = new AuditConfiguration();
         var entity = new TestEntity();
         var userGuid = Guid.NewGuid();

         componentUnderTest.AuditModifiedUserGuid<TestEntity>(c => c.AnotherGuid);
         componentUnderTest.AuditModifiedUserGuid(typeof(TestEntity), new object[] { entity }, userGuid);

         Assert.That(entity.AnotherGuid, Is.EqualTo(userGuid));
      }

      private class TestEntity
      {
         public DateTime DateTime { get; set; }

         public DateTime AnotherDateTime { get; set; }

         public Guid SomeGuid { get; set; }

         public Guid AnotherGuid { get; set; }
      }
   }
}
