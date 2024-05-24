using Microsoft.Data.SqlClient;
using flash_card_api.Libs;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace flash_card_api.Data
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseService> _logger;
 
        public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger) 
        {
            _configuration = configuration;
            _logger = logger;
        }

        public DataTable ExecuteQuery(string sql, string db = "flash_card_db")
        {
            SqlConnection connection = new(_configuration.GetConnectionString(db));
            SqlCommand command = new();
            SqlDataAdapter adapter = new();
            DataSet dataset = new();
            DataTable dataTable = null;

            _logger.LogTrace("Running query: " + sql);
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = sql;
                adapter.SelectCommand = command;
                adapter.Fill(dataset);
                dataTable = dataset.Tables[0];
            }
            catch (SqlException ex)
            {
                _logger.LogWarning("Error running query: " + sql);
                _logger.LogError(ex.ToString());
            }
            finally
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                }
                if (adapter != null)
                    adapter.Dispose();
                if (command != null)
                    command.Dispose();
            }
            return dataTable;
        }

        public bool ExecuteNonQuery(string sql, string db = "flash_card_db")
        {
            bool success = true;
            SqlConnection connection = new(_configuration.GetConnectionString(db));
            SqlCommand command = new();

            _logger.LogTrace("Running query: " + sql);
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error running non-query: " + sql);
                _logger.LogError(ex.ToString());
                success = false;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Dispose();
                }
                if (command != null)
                    command.Dispose();
            }
            return success;
        }

        public static bool IsServerConnected(string connectionString, out string error)
        {
            error = "";
            if (connectionString.IsNullOrWhiteSpace()) { throw new ArgumentNullException("connectionString was null"); }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    error = ex.Message;
                    return false;
                }
            }
        }

        public static void RunMigrations(string master_connection_string, string database_connection_string)
        {
            if (master_connection_string.IsNullOrWhiteSpace() || database_connection_string.IsNullOrWhiteSpace()) { throw new ArgumentNullException("connectionString was null"); }

            // Create the non master DB
             string createDbScript = File.ReadAllText("./Data/create_db.sql");
            using (SqlConnection connection = new SqlConnection(master_connection_string))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(createDbScript, connection);

                command.ExecuteNonQuery();
            }

            // Run all migrations
            string[] migrations = Directory.GetFiles("./Data/migrations");
            foreach (string migration in migrations)
            {
                string script = File.ReadAllText(migration);
                using (SqlConnection connection = new SqlConnection(database_connection_string))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(script, connection);

                    command.ExecuteNonQuery();
                }
            }
        }

        /*** Helper Methods ***/
        public void LogError(string sql, string error)
        {
            if (!string.IsNullOrWhiteSpace(sql))
                _logger.LogWarning("Error running query: " + sql);
            _logger.LogError(error);
        }

        public void LogDebug(string debug)
        {
            _logger.LogWarning(debug);
        }
    }
}
