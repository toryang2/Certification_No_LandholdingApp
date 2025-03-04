using CommonLibrary;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace NoLandholdingApp
{
    public partial class SettingsForm : Form
    {
        private Timer messageTimer;

        public SettingsForm()
        {
            InitializeComponent();
            SetFormProperties();

            messageTimer = new Timer();
            messageTimer.Interval = 3000; // 3 seconds
            messageTimer.Tick += MessageTimer_Tick;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            LoadCurrentAssessor();
        }

        private void SetFormProperties()
        {
            this.Icon = Properties.Resources.setting;
        }

        private void LoadCurrentAssessor()
        {
            var config = ConfigHelper.LoadConfig();

            if (config.Count == 0)
            {
                ShowTemporaryMessage("Database configuration is missing.", Color.Red);
                return;
            }

            string connectionString = $"Server={config["Server"]};Port={config["Port"]};Database={config["Database"]};Uid={config["User"]};Pwd={config["Password"]};";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Assessor FROM sys_signatories ORDER BY ID DESC LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            txtSignatories.Text = result.ToString();
                        }
                        else
                        {
                            txtSignatories.Text = string.Empty;
                            ShowTemporaryMessage("No Assessor found.", Color.Red);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowTemporaryMessage($"Error loading Assessor: {ex.Message}", Color.Red);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var config = ConfigHelper.LoadConfig();

            if (config.Count == 0)
            {
                ShowTemporaryMessage("Database configuration is missing.", Color.Red);
                return;
            }

            string connectionString = $"Server={config["Server"]};Port={config["Port"]};Database={config["Database"]};Uid={config["User"]};Pwd={config["Password"]};";

            string newAssessor = txtSignatories.Text.Trim();

            if (string.IsNullOrEmpty(newAssessor))
            {
                ShowTemporaryMessage("Please enter the Assessor name.", Color.Red);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE sys_signatories 
                        SET Assessor = @Assessor 
                        WHERE ID = (SELECT MAX(ID) FROM sys_signatories)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Assessor", newAssessor);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ShowTemporaryMessage("Assessor updated successfully.", Color.Green);
                        }
                        else
                        {
                            ShowTemporaryMessage("Failed to update Assessor.", Color.Red);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowTemporaryMessage($"Error: {ex.Message}", Color.Red);
            }
        }

        private void ShowTemporaryMessage(string message, Color color)
        {
            lblMessage.ForeColor = color;
            lblMessage.Text = message;

            messageTimer.Stop();
            messageTimer.Start();
        }

        private void MessageTimer_Tick(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            messageTimer.Stop();
        }

        private void databaseSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseSetup dbSetup = new DatabaseSetup();
            dbSetup.ShowDialog();
        }
    }
}
