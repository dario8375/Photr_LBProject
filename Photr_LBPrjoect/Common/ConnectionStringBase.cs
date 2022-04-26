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

                connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Photrn;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            } else {
                connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Photrn;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            }
            return connectionString;
        }
    }
}