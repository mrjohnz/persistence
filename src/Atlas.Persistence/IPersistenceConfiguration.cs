// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPersistenceConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence
{
   public interface IPersistenceConfiguration
   {
      string[] SchemaCreationScript();

      void CreateSchema();
   }
}
