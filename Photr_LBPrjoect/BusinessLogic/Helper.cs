using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Common;
using DataLayer;
using NLog;

namespace BusinessLogic {
    /// <summary>
    /// Class with the Logic for getting/inserting and updating Data
    /// </summary>
    public class Helper {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly bool UnittestConnectionString;

        /// <summary>
        /// Constructor to save ConnectionString to Variable
        /// </summary>
        /// <param name="unittestConnectionString">Variable with the connectionString</param>
        public Helper(bool unittestConnectionString) {
            UnittestConnectionString = unittestConnectionString;
        }

        /// <summary>
        /// Get Data from Table
        /// </summary>
        /// <typeparam name="T">can be any given Model/Table</typeparam>
        /// <returns>all Data from the given Model/Table</returns>
        public List<T> GetData<T>() {
            try {
                SqlCommand command = new DatabaseQueries().CreateTypeQuery<T>();
                ConnectionStringBase connectionStringBase = new ConnectionStringBase();
                string connectionString = connectionStringBase.GetConnectionString(UnittestConnectionString);
                SQLDataAdapter dataAdapter = new SQLDataAdapter(connectionString);
                List<T> data = dataAdapter.RetrieveData<T>(command);
                return data;
            } catch (Exception ex) {
                logger.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Adds the Data to the Database Table
        /// </summary>
        /// <typeparam name="T">can be any given Model/Table</typeparam>
        /// <param name="model">Model with data</param>
        /// <returns>Boolean if Add worked</returns>
        public bool AnnexData<T>(T model) {
            if (model is null) { throw new ArgumentNullException($"{nameof(model)}"); }
            try {
                SqlCommand query = new DatabaseQueries().CreateTypeQuery<T>();
                ImmutableList<SqlParameter> sqlParameters = SqlParameters.GetImmutableInsertParameters<T>(model);
                ConnectionStringBase connectionStringBase = new ConnectionStringBase();
                string connectionString = connectionStringBase.GetConnectionString(UnittestConnectionString);
                SQLDataAdapter dataAdapter = new SQLDataAdapter(connectionString);
                bool persistSuccess = dataAdapter.PersistData<T>(query, sqlParameters, CommandEnum.Add);
                return persistSuccess;
            } catch (Exception ex) {
                logger.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the old Data with the new Data
        /// </summary>
        /// <typeparam name="T">can be any given Model/Table</typeparam>
        /// <param name="oldModel">Model with the old Data</param>
        /// <param name="newModel">Model with the new Data</param>
        /// <returns>Boolean if updating the Data worked</returns>
        public bool EditData<T>(T oldModel, T newModel) {
            if (oldModel is null) { throw new ArgumentNullException($"{nameof(oldModel)}"); }
            if (newModel is null) { throw new ArgumentNullException($"{nameof(newModel)}"); }
            try {
                SqlCommand query = new DatabaseQueries().CreateTypeQuery<T>();
                ConnectionStringBase connectionStringBase = new ConnectionStringBase();
                string connectionString = connectionStringBase.GetConnectionString(UnittestConnectionString);
                SQLDataAdapter dataAdapter = new SQLDataAdapter(connectionString);
                ImmutableList<SqlParameter> sqlParameters = SqlParameters.GetImmutableUpdateParameters<T>(oldModel, newModel);
                bool persistSuccess = dataAdapter.PersistData<T>(query, sqlParameters, CommandEnum.Update);
                return persistSuccess;
            } catch (Exception ex) {
                logger.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets specific Data
        /// </summary>
        /// <typeparam name="T">can be any given Model/Table</typeparam>
        /// <param name="whereData">Id from the given Data</param>
        /// <returns>Data from given Id</returns>
        public List<T> GetSpecificData<T>(string whereData, string whereStatement) {
            try {
                if (string.IsNullOrWhiteSpace(whereData)) { throw new ArgumentOutOfRangeException($"{nameof(whereData)}"); }
                SqlCommand command = new DatabaseQueries().CreateSpecificTypeQuery<T>(whereData, whereStatement);
                ConnectionStringBase connectionStringBase = new ConnectionStringBase();
                string connectionString = connectionStringBase.GetConnectionString(UnittestConnectionString);
                SQLDataAdapter dataAdapter = new SQLDataAdapter(connectionString);
                List<T> data = dataAdapter.RetrieveData<T>(command);
                return data;
            } catch (Exception ex) {
                logger.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets specific Data
        /// </summary>
        /// <typeparam name="T">can be any given Model/Table</typeparam>
        /// <param name="whereData">Id from the given Data</param>
        /// <returns>Data from given Id</returns>
        public List<T> GetSpecificData<T>(string whereData, string andData, string whereStatement, string andStatement) {
            try {
                if (string.IsNullOrWhiteSpace(whereData)) { throw new ArgumentOutOfRangeException($"{nameof(whereData)}"); }
                SqlCommand command = new DatabaseQueries().CreateSpecificTypeQuery<T>(whereData, andData, whereStatement, andStatement);
                ConnectionStringBase connectionStringBase = new ConnectionStringBase();
                string connectionString = connectionStringBase.GetConnectionString(UnittestConnectionString);
                SQLDataAdapter dataAdapter = new SQLDataAdapter(connectionString);
                List<T> data = dataAdapter.RetrieveData<T>(command);
                return data;
            } catch (Exception ex) {
                logger.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets specific Data
        /// </summary>
        /// <typeparam name="T">can be any given Model/Table</typeparam>
        /// <param name="whereData">Id from the given Data</param>
        /// <returns>Data from given Id</returns>
        public List<T> GetSpecificData<T>(string whereData, string orderByData, string whereStatement) {
            try {
                if (string.IsNullOrWhiteSpace(whereData)) { throw new ArgumentOutOfRangeException($"{nameof(whereData)}"); }
                SqlCommand command = new DatabaseQueries().CreateSpecificTypeQuery<T>(whereData, whereStatement, orderByData);
                ConnectionStringBase connectionStringBase = new ConnectionStringBase();
                string connectionString = connectionStringBase.GetConnectionString(UnittestConnectionString);
                SQLDataAdapter dataAdapter = new SQLDataAdapter(connectionString);
                List<T> data = dataAdapter.RetrieveData<T>(command);
                return data;
            } catch (Exception ex) {
                logger.Error(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets specific Data
        /// </summary>
        /// <typeparam name="T">can be any given Model/Table</typeparam>
        /// <param name="whereData">Id from the given Data</param>
        /// <returns>Data from given Id</returns>
        public List<T> GetSpecificDataWithLike<T>(string whereData, string whereStatement) {
            try {
                if (string.IsNullOrWhiteSpace(whereData)) { throw new ArgumentOutOfRangeException($"{nameof(whereData)}"); }
                SqlCommand command = new DatabaseQueries().CreateSpecificTypeQueryWithLike<T>(whereData, whereStatement);
                ConnectionStringBase connectionStringBase = new ConnectionStringBase();
                string connectionString = connectionStringBase.GetConnectionString(UnittestConnectionString);
                SQLDataAdapter dataAdapter = new SQLDataAdapter(connectionString);
                List<T> data = dataAdapter.RetrieveData<T>(command);
                return data;
            }
            catch (Exception ex) {
                logger.Error(ex, ex.Message);
                throw;
            }
        }

        public static string CreateSaltedHash(string password) {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            password = CreateSaltedPassword() + password;
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);

            char[] hash2 = new char[16];

            // Note that here we are wasting bits of hash! 
            // But it isn't really important, because hash.Length == 32
            for (int i = 0; i < hash2.Length; i++) {
                hash2[i] = chars[hash[i] % chars.Length];
            }

            return new string(hash2);
        }

        private static string CreateSaltedPassword() {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[5];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }
    }
}
