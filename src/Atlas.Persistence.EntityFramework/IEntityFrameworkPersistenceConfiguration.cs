// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityFrameworkPersistenceConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.EntityFramework
{
   using System.Data.Entity.Core.Objects;

   public interface IEntityFrameworkPersistenceConfiguration : IPersistenceConfiguration
   {
      ObjectContext CreateObjectContext();
   }
}
