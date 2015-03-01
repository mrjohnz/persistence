// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log4NetPersistenceLoggerShould.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Log4Net.Tests
{
   using System;

   using Atlas.Persistence.Log4Net;

   using FakeItEasy;

   using log4net;

   using NUnit.Framework;

   public class Log4NetPersistenceLoggerShould
   {
      private ILog log;

      private Log4NetPersistenceLogger componentUnderTest;

      [SetUp]
      public void SetupBeforeEachTest()
      {
         this.log = A.Fake<ILog>();

         this.componentUnderTest = new Log4NetPersistenceLogger(this.log);
      }

      [Test]
      public void ImplementIPersistenceLogger()
      {
         Assert.That(this.componentUnderTest, Is.InstanceOf<IPersistenceLogger>());
      }

      [Test]
      public void ReturnLog4NetServiceBusLoggerFromFromConfig()
      {
         var result = Log4NetPersistenceLogger.FromConfig();

         Assert.That(result, Is.InstanceOf<Log4NetPersistenceLogger>());
      }

      [Test]
      public void CallErrorFormatFromLogError()
      {
         const string ErrorMessage = "myErrorMessage '{0}'";
         var args = new object[] { "arg" };
         var exception = new Exception();

         this.componentUnderTest.LogError(ErrorMessage, exception, args);

         A.CallTo(() => this.log.Error("myErrorMessage 'arg'", exception)).MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void CallInfoFormatFromLogWarning()
      {
         const string WarningMessage = "myWarningMessage";
         var args = new object[1];

         this.componentUnderTest.LogWarning(WarningMessage, args);

         A.CallTo(() => this.log.WarnFormat(WarningMessage, args)).MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void CallInfoFormatFromLogInfo()
      {
         const string InfoMessage = "myInfoMessage";
         var args = new object[1];

         this.componentUnderTest.LogInfo(InfoMessage, args);

         A.CallTo(() => this.log.InfoFormat(InfoMessage, args)).MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void CallDebugFormatFromLogDebug()
      {
         const string DebugMessage = "myDebugMessage";
         var args = new object[1];

         this.componentUnderTest.LogDebug(DebugMessage, args);

         A.CallTo(() => this.log.DebugFormat(DebugMessage, args)).MustHaveHappened(Repeated.Exactly.Once);
      }
   }
}
