using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient; // Make sure you installed MySql.Data via NuGet

namespace CommonLibrary
{
    public static class UserRepository
    {
        private static readonly string ConnectionString = BuildConnectionString();

        private static string BuildConnectionString()
        {
            var config = ConfigHelper.LoadConfig();

            if (config == null || config.Count == 0)
                throw new InvalidOperationException("Config file is missing or incomplete.");

            return $"Server={config["Server"]};Port={config["Port"]};Database={config["Database"]};Uid={config["User"]};Pwd={config["Password"]};";
        }

        public static string GetInitialsForUser(string username)
        {
            using (var conn = new MySqlConnection(ConnectionString)) // Correct for MySQL
            {
                conn.Open();
                string query = "SELECT initial FROM sys_login_credentials WHERE Username = @Username";

                using (var cmd = new MySqlCommand(query, conn)) // Correct for MySQL
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString() ?? string.Empty;
                }
            }
        }

        public static int GetAccessLevelForUser(string username)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT accessLevel FROM sys_login_credentials WHERE Username = @Username";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0; // Default to 0 if no record found
                }
            }
        }
    }
}
