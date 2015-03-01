//-----------------------------------------------------------------------
// <copyright file="CompareSchemaTests.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using System.Configuration;

   using Atlas.Persistence.EntityFramework.Implementations;
   using Atlas.Persistence.Log4Net;
   using Atlas.Persistence.Testing.SqlServer;

   using NUnit.Framework;

   public class CompareSchemaTests
   {
      [Test]
      [Category("CompareSchema")]
      [Timeout(300000)]
      public void CompareSchema()
      {
         var superSetConnectionString = ConfigurationManager.ConnectionStrings["Persistence"].ConnectionString;
         var subSetConnectionString = ConfigurationManager.ConnectionStrings["PersistenceEF"].ConnectionString;

         SqlServerSchema.Remove(subSetConnectionString);

         var configuration = new EntityFrameworkDbContextConfiguration<CompareContext>(connectionStringOrName => new CompareContext(connectionStringOrName));
         var logger = Log4NetPersistenceLogger.FromConfig();

         configuration.ConnectionString(subSetConnectionString);
         configuration.CreateSchema();
         
         SqlServerSchema.AssertContained(logger, superSetConnectionString, subSetConnectionString, false, "__MigrationHistory");
      }
   }
}
