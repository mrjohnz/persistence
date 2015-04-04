// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SQLiteAtlasAutoMappingConfiguration.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Testing.Configuration.Fluent
{
   using Atlas.Persistence.NHibernate.Configuration.Fluent;

   using FluentNHibernate;

   public class SQLiteAtlasAutoMappingConfiguration : AtlasAutoMappingConfiguration
   {
      public override bool IsVersion(Member member)
      {
         return false;
      }
   }
}
