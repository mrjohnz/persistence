// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlServerSchema.cs" company="Epworth Consulting Ltd.">
//   © Epworth Consulting Ltd.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Atlas.Persistence.Testing
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Data.SqlClient;
   using System.Linq.Expressions;

   using Microsoft.SqlServer.Management.Smo;

   using NUnit.Framework;

   public static class SqlServerSchema
   {
      public static void Remove(string connectionString)
      {
         Server server;
         string databaseName;
         Remove(connectionString, out server, out databaseName);
      }

      /// <summary>
      /// Asserts that the Sub-set schema is contained within the Super-set schema
      /// </summary>
      public static void AssertContained(IPersistenceLogger persistenceLogger, string superSetConnectionString, string subSetConnectionString, bool compareIndexes, params string[] ignoreTables)
      {
         var logger = new MismatchLogger(persistenceLogger);

         var superSet = GetDatabase(superSetConnectionString);
         var subSet = GetDatabase(subSetConnectionString);
         var boolComparer = new BoolComparer();
         var intComparer = new IntComparer();
         var stringComparer = StringComparer.InvariantCulture;
         var sqlDataTypeComparer = new SqlDataTypeComparer();

         foreach (var subSetTable in subSet.Tables.Cast<Table>())
         {
            if (ignoreTables.Contains(subSetTable.Name))
            {
               logger.LogInfo("Ignoring table '{0}'", subSetTable.Name);

               continue;
            }

            if (!superSet.Tables.Contains(subSetTable.Name))
            {
               logger.LogWarning("Table '{0}' does not exist in '{1}'", subSetTable.Name, superSet.Name);

               continue;
            }

            logger.LogInfo("Comparing table '{0}'", subSetTable.Name);

            var superSetTable = superSet.Tables[subSetTable.Name];
            var superSetColumns = superSetTable.Columns.Cast<Column>().Where(c => !c.Name.EndsWith("NullBuster")).ToDictionary(c => c.Name);
            var subSetColumns = subSetTable.Columns.Cast<Column>().ToDictionary(c => c.Name);

            if (subSetColumns.Count != superSetColumns.Count)
            {
               logger.LogWarning(
                  "Table '{0}' in '{1}' has '{2}' columns compared to '{3}' in '{4}'",
                  subSetTable.Name,
                  subSet.Name,
                  subSetTable.Columns.Count,
                  superSetColumns.Count,
                  superSet.Name);
            }

            foreach (var superSetColumn in superSetColumns)
            {
               if (!subSetColumns.ContainsKey(superSetColumn.Key))
               {
                  logger.LogWarning("Column '{0}' does not exist in '{1}' in '{2}'", superSetColumn.Key, subSetTable.Name, subSet.Name);
               }
            }

            foreach (var subSetColumn in subSetColumns)
            {
               if (!superSetColumns.ContainsKey(subSetColumn.Key))
               {
                  logger.LogWarning("Column '{0}' does not exist in '{1}' in '{2}'", subSetColumn.Key, superSetTable.Name, superSet.Name);

                  continue;
               }

               var superSetColumn = superSetColumns[subSetColumn.Key];

               Compare(logger, subSetTable, subSetColumn.Value, superSetColumn, c => c.DataType.SqlDataType, sqlDataTypeComparer);
               Compare(logger, subSetTable, subSetColumn.Value, superSetColumn, c => c.DataType.MaximumLength, intComparer);
               Compare(logger, subSetTable, subSetColumn.Value, superSetColumn, c => c.DataType.NumericPrecision, intComparer);
               Compare(logger, subSetTable, subSetColumn.Value, superSetColumn, c => c.DataType.NumericScale, intComparer);
               Compare(logger, subSetTable, subSetColumn.Value, superSetColumn, c => c.Nullable, boolComparer);
               Compare(logger, subSetTable, subSetColumn.Value, superSetColumn, c => c.Default, stringComparer);
               Compare(logger, subSetTable, subSetColumn.Value, superSetColumn, c => c.Identity, boolComparer);
               Compare(logger, subSetTable, subSetColumn.Value, superSetColumn, c => c.InPrimaryKey, boolComparer);
            }

            if (compareIndexes)
            {
               var superSetUniqueIndexes = superSetTable.Indexes.Cast<Index>().Where(c => c.IsUnique).ToList();
               var subSetUniqueIndexes = subSetTable.Indexes.Cast<Index>().Where(c => c.IsUnique).ToList();

               if (subSetUniqueIndexes.Count != superSetUniqueIndexes.Count)
               {
                  logger.LogWarning(
                     "Table '{0}' in '{1}' has '{2}' unique indexes compared to '{3}' in '{4}'",
                     subSetTable.Name,
                     subSet.Name,
                     subSetUniqueIndexes.Count,
                     superSetUniqueIndexes.Count,
                     superSet.Name);
               }

               // Primary Keys and Unique Keys can have different names, so can't check for equality

               foreach (var subSetUniqueIndex in subSetUniqueIndexes)
               {
                  var subSetIndexColumns = new HashSet<string>(subSetUniqueIndex.IndexedColumns.Cast<IndexedColumn>().Select(c => c.Name));
                  var found = false;

                  foreach (var superSetUniqueIndex in superSetUniqueIndexes)
                  {
                     var superSetIndexColumns = new HashSet<string>(superSetUniqueIndex.IndexedColumns.Cast<IndexedColumn>().Select(c => c.Name));

                     if (superSetIndexColumns.SetEquals(subSetIndexColumns))
                     {
                        found = true;
                        break;
                     }
                  }

                  if (!found)
                  {
                     logger.LogWarning("Table '{0}' in '{1}' has unexpected unique index over columns '{2}'", subSetTable.Name, subSet.Name, string.Join(",", subSetIndexColumns));
                  }
               }
            }

            // TODO: Check FK (keys can have different names)
         }

         if (logger.Errors || logger.Warnings)
         {
            Assert.Fail("Schema comparison failed - see ILog output for details");
         }
      }

      private static Database GetDatabase(string connectionString)
      {
         Server server;
         string databaseName;

         var database = GetDatabase(connectionString, out server, out databaseName);

         if (database == null)
         {
            throw new InvalidOperationException(string.Format("Database not found - {0}", connectionString));
         }

         return database;
      }

      private static Database GetDatabase(string connectionString, out Server server, out string databaseName)
      {
         var sqlConnection = new SqlConnection(connectionString);

         server = new Server(sqlConnection.DataSource);
         databaseName = sqlConnection.Database;

         if (!server.Databases.Contains(databaseName))
         {
            return null;
         }

         return server.Databases[databaseName];
      }

      private static void Remove(string connectionString, out Server server, out string databaseName)
      {
         var sqlConnection = new SqlConnection(connectionString);
         server = new Server(sqlConnection.DataSource);
         databaseName = sqlConnection.Database;

         if (server.Databases.Contains(databaseName))
         {
            server.KillAllProcesses(databaseName);
            server.Databases[databaseName].Drop();
         }
      }

      private static void Compare<T>(IPersistenceLogger logger, Table table, Column tableColumn, Column otherColumn, Expression<Func<Column, T>> attributeExpression, IEqualityComparer<T> comparer)
      {
         var propertyExpression = attributeExpression.Body as MemberExpression;

         if (propertyExpression == null)
         {
            throw new ArgumentException("Expression must be of a property", "attributeExpression");
         }

         var attribute = attributeExpression.Compile();
         var tableArg = attribute(tableColumn);
         var otherArg = attribute(otherColumn);

         if (!comparer.Equals(tableArg, otherArg))
         {
            logger.LogWarning("Attribute {0} of column '{1}' in '{2}' is '{3}' compared to '{4}'", propertyExpression.Member.Name, tableColumn.Name, table.Name, tableArg, otherArg);
         }
      }

      private class MismatchLogger : IPersistenceLogger
      {
         private readonly IPersistenceLogger wrappedLog;

         public MismatchLogger(IPersistenceLogger wrappedLog)
         {
            this.wrappedLog = wrappedLog;
         }

         public bool Warnings { get; private set; }

         public bool Errors { get; private set; }

         public void LogDebug(string message, params object[] args)
         {
            this.wrappedLog.LogDebug(message, args);
         }

         public void LogInfo(string message, params object[] args)
         {
            this.wrappedLog.LogInfo(message, args);
         }

         public void LogWarning(string message, params object[] args)
         {
            this.wrappedLog.LogWarning(message, args);
            this.Warnings = true;
         }

         public void LogError(string message, Exception exception, params object[] args)
         {
            this.wrappedLog.LogError(message, exception, args);
            this.Errors = true;
         }
      }

      private class BoolComparer : IEqualityComparer<bool>
      {
         public bool Equals(bool x, bool y)
         {
            return x == y;
         }

         public int GetHashCode(bool obj)
         {
            return obj.GetHashCode();
         }
      }

      private class IntComparer : IEqualityComparer<int>
      {
         public bool Equals(int x, int y)
         {
            return x == y;
         }

         public int GetHashCode(int obj)
         {
            return obj.GetHashCode();
         }
      }

      private class SqlDataTypeComparer : IEqualityComparer<SqlDataType>
      {
         public bool Equals(SqlDataType x, SqlDataType y)
         {
            return x == y;
         }

         public int GetHashCode(SqlDataType obj)
         {
            return obj.GetHashCode();
         }
      }
   }
}
