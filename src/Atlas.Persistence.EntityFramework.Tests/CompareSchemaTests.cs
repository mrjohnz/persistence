//-----------------------------------------------------------------------
// <copyright file="CompareSchemaTests.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests
{
   using System.Configuration;

   using Atlas.Domain.Persistence.EntityFramework.Tests;
   using Atlas.Persistence.EntityFramework.Implementations;
   using Atlas.Persistence.Testing;

   using FakeItEasy;

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

         var configuration = new EntityFrameworkConfiguration();
         var logger = A.Fake<IPersistenceLogger>();

         configuration.ConnectionString(subSetConnectionString);
         configuration.ProviderName(EntityFrameworkConfiguration.SqlServerProviderName);
         configuration.RegisterEntitiesFromAssemblyOf<FooConfiguration>();
         configuration.CreateSchema();

         SqlServerSchema.AssertContained(logger, superSetConnectionString, subSetConnectionString, false, "EdmMetadata");
      }
   }
}
