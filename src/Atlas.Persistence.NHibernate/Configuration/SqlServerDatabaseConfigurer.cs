// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlServerDatabaseConfigurer.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.NHibernate.Configuration
{
   using System;
   using System.Configuration;

   using Atlas.Persistence.NHibernate;

   using global::NHibernate.Dialect;
   using global::NHibernate.Driver;

   using Configuration = global::NHibernate.Cfg.Configuration;

   public class SqlServerDatabaseConfigurer : INHibernateConfigurer
   {
      private const string DriverKey = "connection.driver_class";
      private const string DialectKey = "dialect";
      private const string ConnectionStringKey = "connection.connection_string";
      private const string DefaultSchemaKey = "default_schema";
      private const string UseReflectionOptimizerKey = "use_reflection_optimizer";

      private string connectionString;
      private Type driverType;
      private Type dialectType;

      public SqlServerDatabaseConfigurer()
      {
         this.driverType = typeof(Sql2008ClientDriver);
         this.dialectType = typeof(MsSql2012Dialect);
      }

      // ReSharper disable once ParameterHidesMember
      public SqlServerDatabaseConfigurer ConnectionString(string connectionString)
      {
         this.connectionString = connectionString;

         return this;
      }

      public SqlServerDatabaseConfigurer ConnectionStringName(string connectionStringName)
      {
         var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

         if (connectionStringSettings == null)
         {
            throw new ArgumentException(string.Format("ConnectionStringName '{0}' not found", connectionStringName));
         }

         this.connectionString = connectionStringSettings.ConnectionString;

         return this;
      }

      public SqlServerDatabaseConfigurer DriverType<TDriver>()
         where TDriver : SqlClientDriver
      {
         this.driverType = typeof(TDriver);

         return this;
      }

      // ReSharper disable once ParameterHidesMember
      public SqlServerDatabaseConfigurer DriverType(Type driverType)
      {
         if (!typeof(SqlClientDriver).IsAssignableFrom(driverType))
         {
            throw new ArgumentOutOfRangeException("driverType");
         }

         this.driverType = driverType;

         return this;
      }

      public SqlServerDatabaseConfigurer DialectType<TDialect>()
         where TDialect : Dialect
      {
         this.dialectType = typeof(TDialect);

         return this;
      }

      // ReSharper disable once ParameterHidesMember
      public SqlServerDatabaseConfigurer DialectType(Type dialectType)
      {
         if (!typeof(Dialect).IsAssignableFrom(dialectType))
         {
            throw new ArgumentOutOfRangeException("dialectType");
         }

         this.dialectType = dialectType;

         return this;
      }

      public void Configure(Configuration configuration)
      {
         configuration.SetProperty(DriverKey, this.driverType.AssemblyQualifiedName);
         configuration.SetProperty(DialectKey, this.dialectType.AssemblyQualifiedName);
         configuration.SetProperty(ConnectionStringKey, this.connectionString);
         configuration.SetProperty(DefaultSchemaKey, "dbo");
         configuration.SetProperty(UseReflectionOptimizerKey, "true");
      }
   }
}
