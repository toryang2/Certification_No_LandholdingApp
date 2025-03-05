using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;


namespace CommonLibrary
{
    public partial class DatabaseSetup : Form
    {
        public bool ConfigSaved { get; private set; } = false;
        public DatabaseSetup()
        {
            InitializeComponent();
            LoadDatabaseSettings();  // Store the reference of the formInput
            btnSave.Enabled = false; // Initially disable the save button

            this.KeyPreview = true;
            this.KeyDown += DatabaseSetup_KeyDown;
            //this.Icon = Properties.Resources.dataserver;
        }

        private void checkBoxEnableDatabaseEdit_CheckedChanged(object sender, EventArgs e)
        {
            // Enable or disable the textbox based on checkbox state
            txtDatabase.Enabled = checkBoxEnableDatabaseEdit.Checked;
        }

        private void DatabaseSetup_Load(object sender, EventArgs e)
        {
            // Ensure txtDatabase is disabled initially
            txtDatabase.Enabled = false;

            // Optional: Ensure checkbox is unchecked initially
            checkBoxEnableDatabaseEdit.Checked = false;

            // Attach event handler for checkboxa
            checkBoxEnableDatabaseEdit.CheckedChanged += checkBoxEnableDatabaseEdit_CheckedChanged;
        }

        private void LoadDatabaseSettings()
        {
            var config = ConfigHelper.LoadConfig();

            if (config != null && config.Count > 0)
            {
                txtServer.Text = config.ContainsKey("Server") ? config["Server"] : "";
                txtDatabase.Text = config.ContainsKey("Database") ? config["Database"] : "";
                txtUser.Text = config.ContainsKey("User") ? config["User"] : "";
                txtPassword.Text = config.ContainsKey("Password") ? config["Password"] : "";
                txtPort.Text = config.ContainsKey("Port") ? config["Port"] : "";

                btnSave.Enabled = false; // Initially disable save until test is successful
            }
            else
            {
                MessageBox.Show("Configuration file is missing or empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var config = new Dictionary<string, string>
    {
        { "Server", txtServer.Text.Trim() },
        { "Database", txtDatabase.Text.Trim() },
        { "User", txtUser.Text.Trim() },
        { "Password", txtPassword.Text.Trim() },
        { "Port", txtPort.Text.Trim() }
    };

            ConfigHelper.SaveConfig(config);

            // Check if saved correctly
            if (ConfigHelper.LoadConfig() != null)
            {
                MessageBox.Show("Config file has been saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Focus();
                ConfigSaved = true; // Indicate success
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to save config file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {

        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            string connectionString = $"Server={txtServer.Text.Trim()};Port={txtPort.Text.Trim()};Database={txtDatabase.Text.Trim()};Uid={txtUser.Text.Trim()};Pwd={txtPassword.Text.Trim()};";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Database connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = true; // Enable save button if connection succeeds
                    btnSave.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false; // Keep save button disabled if connection fails
            }
        }

        private void DatabaseSetup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Trigger the btnTestConnection_Click event when Enter is pressed
                btnTestConnection_Click(sender, e);
            }
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
