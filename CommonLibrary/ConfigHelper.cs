using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibrary
{
    public class ConfigHelper
    {
        private static readonly string configPath = "config.ini";

        /// <summary>
        /// Loads the configuration from the config.ini file.
        /// </summary>
        /// <returns>A dictionary containing configuration key-value pairs or null if the file is missing.</returns>
        public static Dictionary<string, string> LoadConfig()
        {
            if (!File.Exists(configPath))
            {
                return null; // Indicate that the config file is missing
            }

            var config = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines(configPath))
            {
                if (!string.IsNullOrWhiteSpace(line) && line.Contains('=') && !line.StartsWith("["))
                {
                    var parts = line.Split(new[] { '=' }, 2); // Split into key and value
                    config[parts[0].Trim()] = parts[1].Trim();
                }
            }

            return config;
        }

        /// <summary>
        /// Saves the given configuration dictionary to the config.ini file.
        /// </summary>
        public static void SaveConfig(Dictionary<string, string> config)
        {
            if (config == null || config.Count == 0)
                return; // Do nothing if the config is empty

            // Ensure the file exists before writing
            if (!File.Exists(configPath))
            {
                File.Create(configPath).Close(); // Create and close to avoid locking issues
            }

            var lines = new List<string> { "[Database]" };
            lines.AddRange(config.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            File.WriteAllLines(configPath, lines);
        }
    }
}
