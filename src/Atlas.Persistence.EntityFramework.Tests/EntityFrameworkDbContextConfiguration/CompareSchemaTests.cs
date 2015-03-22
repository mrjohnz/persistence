//-----------------------------------------------------------------------
// <copyright file="CompareSchemaTests.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests.EntityFrameworkDbContextConfiguration
{
   using System.Configuration;

   using Atlas.Core.Logging;
   using Atlas.Persistence.EntityFramework.Implementations;
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

         configuration.ConnectionString(subSetConnectionString);
         configuration.CreateSchema();
         
         SqlServerSchema.AssertContained(new ConsoleLogger(), superSetConnectionString, subSetConnectionString, false, "__MigrationHistory");
      }
   }
}
