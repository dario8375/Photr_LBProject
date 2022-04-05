using NLog;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Common {
    /// <summary>
    /// Class with all Queries that can be used
    /// </summary>
    public class DatabaseQueries {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Method to create query for a specific Data
        /// </summary>
        /// <typeparam name="T">Give in Model to get query for that table</typeparam>
        /// <param name="whereData">id for specific data</param>
        /// <returns>Query for specific type</returns>
        public SqlCommand CreateSpecificTypeQuery<T>(string whereData, string whereStatement) {
            if (string.IsNullOrWhiteSpace(whereData)) { throw new ArgumentOutOfRangeException($"{nameof(whereData)} zero values are not allowed"); }
            string type = string.Empty;
            string inputType = typeof(T).ToString();
            try {
                type = inputType.Split(".").Last();
            } catch (ArgumentOutOfRangeException ex) {
                Logger.Error(ex, ex.Message);
                throw;
            } catch (Exception ex) {
                Logger.Error(ex, ex.Message);
            }
            string query = $"SELECT * FROM dbo.{type} WHERE {whereStatement} = @whereData";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@whereData", whereData);
            return command;
        }

        /// <summary>
        /// Method to create query for a specific Data
        /// </summary>
        /// <typeparam name="T">Give in Model to get query for that table</typeparam>
        /// <param name="whereData">id for specific data</param>
        /// <returns>Query for specific type</returns>

        /// <summary>
        /// Method to create query for a specific Data
        /// </summary>
        /// <typeparam name="T">Give in Model to get query for that table</typeparam>
        /// <param name="whereData">id for specific data</param>
        /// <returns>Query for specific type</returns>
        public SqlCommand CreateSpecificTypeQuery<T>(string whereData, string andData, string whereStatement, string andStatement) {
            if (string.IsNullOrWhiteSpace(whereData)) { throw new ArgumentOutOfRangeException($"{nameof(whereData)} zero values are not allowed"); }
            string type = string.Empty;
            string inputType = typeof(T).ToString();
            try {
                type = inputType.Split(".").Last();
            } catch (ArgumentOutOfRangeException ex) {
                Logger.Error(ex, ex.Message);
                throw;
            } catch (Exception ex) {
                Logger.Error(ex, ex.Message);
            }
            string query = $"SELECT * FROM dbo.{type} WHERE {whereStatement} = @whereData AND {andStatement} = @andData";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@whereData", whereData);
            command.Parameters.AddWithValue("@andData", andData);
            return command;
        }

        /// <summary>
        /// Method to create query for a specific Data
        /// </summary>
        /// <typeparam name="T">Give in Model to get query for that table</typeparam>
        /// <param name="whereData">id for specific data</param>
        /// <returns>Query for specific type</returns>
        public SqlCommand CreateSpecificTypeQuery<T>(string whereData, string orderByData, string whereStatement) {
            if (string.IsNullOrWhiteSpace(whereData)) { throw new ArgumentOutOfRangeException($"{nameof(whereData)} zero values are not allowed"); }
            string type = string.Empty;
            string inputType = typeof(T).ToString();
            try {
                type = inputType.Split(".").Last();
            } catch (ArgumentOutOfRangeException ex) {
                Logger.Error(ex, ex.Message);
                throw;
            } catch (Exception ex) {
                Logger.Error(ex, ex.Message);
            }
            string query = $"SELECT * FROM dbo.{type} WHERE {whereStatement} = @whereData ORDER BY {orderByData}";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@whereData", whereData);
            return command;
        }

        /// <summary>
        /// Method to create query for a specific Data
        /// </summary>
        /// <typeparam name="T">Give in Model to get query for that table</typeparam>
        /// <param name="whereData">id for specific data</param>
        /// <returns>Query for specific type</returns>
        public SqlCommand CreateSpecificTypeQueryWithLike<T>(string whereData, string whereStatement) {
            if (string.IsNullOrWhiteSpace(whereData)) { throw new ArgumentOutOfRangeException($"{nameof(whereData)} zero values are not allowed"); }
            string type = string.Empty;
            string inputType = typeof(T).ToString();
            try {
                type = inputType.Split(".").Last();
            }
            catch (ArgumentOutOfRangeException ex) {
                Logger.Error(ex, ex.Message);
                throw;
            }
            catch (Exception ex) {
                Logger.Error(ex, ex.Message);
            }
            string query = $"SELECT * FROM dbo.{type} WHERE {whereStatement} like @whereData";
            SqlCommand command = new SqlCommand(query);
            command.Parameters.AddWithValue("@whereData", "%" + whereData + "%");
            return command;
        }
        /// <summary>
        /// Method to create query to get all Data
        /// </summary>
        /// <typeparam name="T">Give in Model to get query for that table</typeparam>
        /// <returns>Query from given Type</returns>
        public SqlCommand CreateTypeQuery<T>() {
            string type = string.Empty;
            string inputType = typeof(T).ToString();
            try {
                type = inputType.Split(".").Last();
            } catch (ArgumentOutOfRangeException ex) {
                Logger.Error(ex, ex.Message);
                throw;
            } catch (Exception ex) {
                Logger.Error(ex, ex.Message);
            }
            string query = $"SELECT * FROM {type}";
            SqlCommand command = new SqlCommand(query);
            return command;
        }
    }
}
