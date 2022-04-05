using System.IO;

namespace Common {
    /// <summary>
    /// Class with the ConnectionString
    /// </summary>
    public class ConnectionStringBase {

        /// <summary>
        /// Gets Relative Path from MockDatabase
        /// </summary>
        /// <returns>Path from MockDatabse</returns>
        public string GetConnectionString(bool unittestConnectionString) {
            string connectionString;
            if (unittestConnectionString) {
                connectionString = @"";
            } else {
                connectionString = @"";
            }
            return connectionString;
        }
    }
}