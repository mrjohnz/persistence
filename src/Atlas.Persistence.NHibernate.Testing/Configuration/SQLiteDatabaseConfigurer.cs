// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SQLiteDatabaseConfigurer.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Testing.Configuration
{
   using Atlas.Persistence.NHibernate;

   using global::NHibernate.Cfg;
   using global::NHibernate.Dialect;
   using global::NHibernate.Driver;

   // ReSharper disable once InconsistentNaming
   public class SQLiteDatabaseConfigurer : INHibernateConfigurer
   {
      public const string InMemoryConnectionString = "Data Source=:memory:;Version=3;New=True;";

      private const string DriverKey = "connection.driver_class";
      private const string DialectKey = "dialect";
      private const string ConnectionStringKey = "connection.connection_string";
      private const string ConnectionReleaseMode = "connection.release_mode";
      private const string QuerySubstitutions = "query.substitutions";

      public void Configure(Configuration configuration)
      {
         configuration.DataBaseIntegration(c =>
            {
               c.Driver<SQLite20Driver>();
               c.Dialect<SQLiteDialect>();
            });

         configuration.SetProperty(DriverKey, typeof(SQLite20Driver).AssemblyQualifiedName);
         configuration.SetProperty(DialectKey, typeof(SQLiteDialect).AssemblyQualifiedName);
         configuration.SetProperty(ConnectionStringKey, InMemoryConnectionString);
         configuration.SetProperty(ConnectionReleaseMode, "on_close");
         configuration.SetProperty(QuerySubstitutions, "true=1;false=0");
      }
   }
}
