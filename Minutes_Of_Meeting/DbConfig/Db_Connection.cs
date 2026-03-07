using Microsoft.Data.SqlClient;

namespace Minutes_Of_Meeting.DbConfig
{
    public class Db_Connection
    {
        private readonly string _connectionString;

        public Db_Connection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("connectionString");

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}