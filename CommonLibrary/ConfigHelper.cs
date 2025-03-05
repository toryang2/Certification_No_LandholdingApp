using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibrary
{
    public class ConfigHelper
    {
        private static readonly string CompanyName = "LGU Kitaotao";
        private static readonly string AppName = "Assessor Certifcate";
        private static readonly string ConfigFileName = "config.ini";

        // MSIX-compatible config path
        private static readonly string LocalConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            CompanyName,
            AppName,
            ConfigFileName
        );

        // Original install location path (read-only in MSIX)
        private static readonly string OriginalConfigPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            ConfigFileName
        );

        /// <summary>
        /// Initializes the config system, migrating existing config if needed
        /// </summary>
        static ConfigHelper()
        {
            InitializeConfigFolder();
        }

        private static void InitializeConfigFolder()
        {
            var configDirectory = Path.GetDirectoryName(LocalConfigPath);

            // Ensure local config directory exists
            Directory.CreateDirectory(configDirectory);

            // Migrate config from original location if needed
            if (!File.Exists(LocalConfigPath) && File.Exists(OriginalConfigPath))
            {
                File.Copy(OriginalConfigPath, LocalConfigPath, overwrite: false);
            }
        }

        public static Dictionary<string, string> LoadConfig()
        {
            if (!File.Exists(LocalConfigPath))
            {
                return null; // Config doesn't exist in either location
            }

            var config = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines(LocalConfigPath))
            {
                if (!string.IsNullOrWhiteSpace(line) &&
                    line.Contains('=') &&
                    !line.StartsWith("["))
                {
                    var parts = line.Split(new[] { '=' }, 2);
                    config[parts[0].Trim()] = parts[1].Trim();
                }
            }

            return config;
        }

        public static void SaveConfig(Dictionary<string, string> config)
        {
            if (config == null || config.Count == 0)
                return;

            var lines = new List<string> { "[Database]" };
            lines.AddRange(config.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            File.WriteAllLines(LocalConfigPath, lines);
        }

        public static string GetConfigPath()
        {
            return LocalConfigPath;
        }
    }
}