//-----------------------------------------------------------------------
// <copyright file="CompareSchemaTests.cs" company="Epworth Consulting Ltd.">
//     © Epworth Consulting Ltd.
// </copyright>
//-----------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework.Tests
{
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
         const string SuperSetConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=AtlasPersistenceTests;Integrated Security=True;MultipleActiveResultSets=True";
         const string SubSetConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=AtlasPersistenceTests_EntityFramework;Integrated Security=True;MultipleActiveResultSets=True";

         SqlServerSchema.Remove(SubSetConnectionString);

         var configuration = new EntityFrameworkConfiguration();
         var logger = A.Fake<IPersistenceLogger>();

         configuration.ConnectionString(SubSetConnectionString);
         configuration.ProviderName(EntityFrameworkConfiguration.SqlServerProviderName);
         configuration.RegisterEntitiesFromAssemblyOf<FooConfiguration>();
         configuration.CreateSchema();

         SqlServerSchema.AssertContained(logger, SuperSetConnectionString, SubSetConnectionString, false, "EdmMetadata");
      }
   }
}
