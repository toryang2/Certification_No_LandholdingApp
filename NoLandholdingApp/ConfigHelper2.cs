using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

public class ConfigHelper2
{
    private static string configPath = "config.ini";
    private static bool messageShown = false;  // Prevent multiple message boxes

    public static Dictionary<string, string> LoadConfig()
    {
        var config = new Dictionary<string, string>();

        if (!File.Exists(configPath))
        {
            if (!messageShown)
            {
                MessageBox.Show("Config file not found! Please enter your database settings.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                messageShown = true; // Ensure message only shows once
            }
            return config;
        }

        foreach (var line in File.ReadAllLines(configPath))
        {
            if (!string.IsNullOrWhiteSpace(line) && line.Contains('=') && !line.StartsWith("["))
            {
                var parts = line.Split('=');
                config[parts[0].Trim()] = parts[1].Trim();
            }
        }

        return config;
    }

    public static void SaveConfig(Dictionary<string, string> config)
    {
            // Ensure the file exists before writing
            if (!File.Exists(configPath))
            {
                File.Create(configPath).Close();  // Create the file and close it
            }

            var lines = new List<string> { "[Database]" };
            lines.AddRange(config.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            File.WriteAllLines(configPath, lines);

        
    }
}
