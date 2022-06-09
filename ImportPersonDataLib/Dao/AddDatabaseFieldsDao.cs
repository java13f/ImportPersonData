using ImportPersonDataLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ImportPersonDataLib.Dao
{
    /// <summary>
    /// Добавление в БД двух полей: isImport, errorMessage
    /// </summary>
    class AddDatabaseFieldsDao : Utils, IAddDatabaseFieldsDao
    {
        private readonly string connectionString;
        private readonly string sql;

        public AddDatabaseFieldsDao(string sql, string connectionString)
        {
            this.sql = sql;
            this.connectionString = connectionString;
        }


        public bool AddFields()
        {
            using (var context = new ImportPersonContext(connectionString))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {                        
                        var nameTable = GetNameTable(sql);
                        var schemaTable = GetSchemaTable(context, nameTable);
                        AddFields(context, schemaTable, nameTable);

                        transaction.Commit();
                        return true;
                    }
                    catch(FieldAccessException ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Method: AddFields; Error: {ex.Message}."); 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Method: AddFields; Error: {ex.Message}");
                    }
                }
            }
        }

        private string GetSchemaTable(ImportPersonContext context, string nameTable)
        {
            string sql = "SELECT t.TABLE_SCHEMA AS [Schema] " +
                        "FROM INFORMATION_SCHEMA.TABLES AS t " +
                        "WHERE t.TABLE_NAME = @nameTable";
                                    
            var schemaTable = context.Database.SqlQuery<string>(sql, 
                        new SqlParameter { ParameterName = "@nameTable", Value = nameTable }).ToList()[0];

            if (string.IsNullOrWhiteSpace(schemaTable))
            {
                throw new FieldAccessException("Ошибка получения схемы таблицы.");
            }

            return schemaTable;
        }

        private void AddFields(ImportPersonContext context, string schemaTable, string nameTable)
        {
            string sql = 
                $"IF COL_LENGTH('{schemaTable}.{nameTable}', 'isImport') IS NULL " +
                "BEGIN " +
                $"   ALTER TABLE {nameTable} " +
                "    ADD isImport bit NULL " +                                
                "END " + 
                
                $"IF COL_LENGTH('{schemaTable}.{nameTable}', 'errorMessage') IS NULL " +
                "BEGIN " +
                $"   ALTER TABLE {nameTable} " +
                "    ADD errorMessage varchar(255) NULL " +                
                "END";
                        
            try
            {
                context.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception ex)
            {
                throw new FieldAccessException($"Ошибка при добавлении столбцов. {ex.Message}");
            }            
        }


    }
}
