sqlcmd -S .\SQLEXPRESS -v path="C:\Work\Databases" -v database="AtlasPersistenceTests" -i Create.sql

sqlcmd -S .\SQLEXPRESS -v database="AtlasPersistenceTests" -i Tables.sql
sqlcmd -S .\SQLEXPRESS -v database="AtlasPersistenceTests" -i PrimaryKeys.sql
sqlcmd -S .\SQLEXPRESS -v database="AtlasPersistenceTests" -i UniqueKeys.sql
sqlcmd -S .\SQLEXPRESS -v database="AtlasPersistenceTests" -i ForeignKeys.sql

pause
