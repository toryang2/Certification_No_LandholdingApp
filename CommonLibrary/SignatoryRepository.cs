using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace CommonLibrary
{
    public static class SignatoryRepository
    {
        private static readonly string ConnectionString = BuildConnectionString();

        private static string BuildConnectionString()
        {
            var config = ConfigHelper.LoadConfig();

            if (config == null || config.Count == 0)
                throw new InvalidOperationException("Config file is missing or incomplete.");

            return $"Server={config["Server"]};Port={config["Port"]};Database={config["Database"]};Uid={config["User"]};Pwd={config["Password"]};";
        }

        public static string GetLatestAssessor()
        {
            string latestAssessor = string.Empty;

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT Assessor FROM sys_signatories ORDER BY ID DESC LIMIT 1";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    latestAssessor = result?.ToString() ?? string.Empty;
                }
            }

            return latestAssessor;
        }
    }
}
