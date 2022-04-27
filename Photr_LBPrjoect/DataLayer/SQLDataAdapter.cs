using Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace DataLayer {
    /// <summary>
    /// Class with all the Code to Add/Update or Get Data from Database
    /// </summary>
    public class SQLDataAdapter {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        string connectionString = string.Empty;

        private static string GetConnectionString() {
            return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Photrn;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        private static List<T> ConvertDataTable<T>(DataTable results) {
            List<T> dataRows = new List<T>();
            foreach (DataRow row in results.Rows) {
                T item = GetItem<T>(row);
                dataRows.Add(item);
            }
            return dataRows;
        }

        private static T GetItem<T>(DataRow dataRow) {
            Type properties = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dataRow.Table.Columns) {
                foreach (PropertyInfo property in properties.GetProperties()) {
                    if (property.Name.Equals(column.ColumnName, StringComparison.InvariantCultureIgnoreCase)) {
                        if (dataRow[column.ColumnName] == null || dataRow[column.ColumnName] == DBNull.Value) {
                            property.SetValue(obj, null, index: null);
                        } else {
                            property.SetValue(obj, dataRow[column.ColumnName], index: null);
                        }
                    } else {
                        continue;
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// Constructor which sets the connectionString
        /// </summary>
        /// <param name="connectionString">string with the connectionString</param>
        /// <exception cref="ArgumentNullException">throws ArgumentNullException</exception>
        public SQLDataAdapter(string connectionString) {
            if (string.IsNullOrWhiteSpace(connectionString)) {
                throw new ArgumentNullException($"{nameof(connectionString)}");
            }
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Retrieves Data
        /// </summary>
        /// <param name="command">the Select Query</param>
        /// <typeparam name="T">Type which can be any given Model</typeparam>
        /// <exception cref="Exception">throws any Exception</exception>
        /// <exception cref="ArgumentNullException">throws ArgumentNullException</exception>
        /// <returns>List with Data</returns>
        public List<T> RetrieveData<T>(SqlCommand command) {
            if (command is null) { throw new ArgumentNullException($"{nameof(command)}"); }
            try {
                using var connection = new SqlConnection(connectionString);
                DataTable results = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command.CommandText, connection)) {
                    command.Connection = connection;
                    adapter.SelectCommand = command;
                    connection.Open();
                    adapter.Fill(results);
                }
                //if(results.Rows.Count == 0) { throw new ArgumentNullException($"{nameof(results)}"); }
                return ConvertDataTable<T>(results);
            } catch (Exception ex) {
                Logger.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// In this Method the Data gets either Added to the database or Updated
        /// </summary>
        /// <typeparam name="T">Type which can be any given Model</typeparam>
        /// <param name="command">param with the query</param>
        /// <param name="sqlParameters">param with the sqlParameters</param>
        /// <param name="whatCommand">param if Data should be added or updated</param>
        /// <returns>Boolean if adding or updating worked</returns>
        public bool PersistData<T>(SqlCommand command, ImmutableList<SqlParameter> sqlParameters, CommandEnum whatCommand) {
            if (command is null) { throw new ArgumentNullException($"{nameof(command)}"); }
            if (sqlParameters is null) { throw new ArgumentNullException($"{nameof(sqlParameters)}"); }
            bool success = false;
            using var connection = new SqlConnection(connectionString);
            using SqlDataAdapter adapter = new SqlDataAdapter(command.CommandText, connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            connection.Open();
            SqlTransaction transaction = null;
            int count;
            switch (whatCommand) {
                case CommandEnum.Add:
                    using (SqlCommand insertCommand = builder.GetInsertCommand()) {
                        try {
                            transaction = connection.BeginTransaction();
                            insertCommand.Transaction = transaction;
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddRange(sqlParameters.ToArray());
                            count = insertCommand.ExecuteNonQuery();
                            transaction.Commit();
                            if (count > 0) {
                                success = true;
                            }
                        } catch (Exception ex) {
                            Logger.Error(ex);
                            try {
                                transaction.Rollback();
                            } catch (Exception exRollback) {
                                Logger.Error(exRollback, exRollback.Message);
                                throw;
                            }
                            throw;
                        }
                    }
                    break;
                case CommandEnum.Update:
                    using (SqlCommand updateCommand = builder.GetUpdateCommand(true)) {
                        try {
                            transaction = connection.BeginTransaction();
                            updateCommand.Transaction = transaction;
                            updateCommand.Parameters.Clear();
                            updateCommand.Parameters.AddRange(sqlParameters.ToArray());
                            count = updateCommand.ExecuteNonQuery();
                            transaction.Commit();
                            if (count > 0) {
                                success = true;
                            }
                        } catch (Exception ex) {
                            Logger.Error(ex);
                            try {
                                transaction.Rollback();
                            } catch (Exception exRollback) {
                                Logger.Error(exRollback, exRollback.Message);
                                throw;
                            }
                            throw;
                        }
                    }
                    break;
                default:
                    throw new NotSupportedException($"{nameof(whatCommand)}, {whatCommand}");
            }
            return success;
        }
    }
}
