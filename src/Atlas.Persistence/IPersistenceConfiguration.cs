// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPersistenceConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   using System.Data;

   public interface IPersistenceConfiguration
   {
      string[] SchemaCreationScript();

      void CreateSchema();

      void CreateSchema(IDbConnection connection);
   }
}
