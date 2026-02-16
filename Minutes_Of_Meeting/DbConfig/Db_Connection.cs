using Microsoft.Data.SqlClient;

namespace Minutes_Of_Meeting.DbConfig
{
    public class Db_Connection
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public Db_Connection(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            this.connectionString = GetWorkingConnectionString();
        }
        public string GetWorkingConnectionString()
        {
            string connectionString = configuration.GetConnectionString("connectionString");

            if(string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured.");
            }
            try
            {

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                    Console.WriteLine("Database connection successful.");
                    return connectionString;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database connection failed: " + ex.Message);
                throw;
            }
            throw new InvalidOperationException("Unable to establish a database connection.");
        }
    }
}
