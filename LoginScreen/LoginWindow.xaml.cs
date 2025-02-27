using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using CommonLibrary;
using System.Windows.Controls;

namespace LoginScreen
{
    public partial class LoginWindow : Window
    {
        public bool IsAuthenticated { get; private set; } = false; // Track login status
        private Dictionary<string, string> config; // Store configuration settings
        private bool isCreatingAccount = false; // Track mode (login or create account)

        public LoginWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => DragMove(); // Allows dragging

            // Load configuration
            config = ConfigHelper.LoadConfig();
            if (config == null || !config.ContainsKey("Server") || !config.ContainsKey("Database"))
            {
                MessageBox.Show("Config file is missing or incomplete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            // Placeholder logic
            txtUsername.GotFocus += (s, e) => PlaceholderText.Visibility = Visibility.Collapsed;
            txtUsername.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                    PlaceholderText.Visibility = Visibility.Visible;
            };

            txtPassword.GotFocus += (s, e) => PlaceholderTextPassword.Visibility = Visibility.Collapsed;
            txtPassword.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                    PlaceholderTextPassword.Visibility = Visibility.Visible;
            };

            // Load "Remember Me" feature
            if (config.ContainsKey("RememberMe") && config["RememberMe"] == "true")
            {
                txtUsername.Text = config.ContainsKey("SavedUsername") ? config["SavedUsername"] : "";
                checkBoxRememberMe.IsChecked = true;
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            string connectionString = $"Server={config["Server"]};Database={config["Database"]};User ID={config["User"]};Password={config["Password"]};";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM sys_login_credentials WHERE username = @username AND password = SHA2(@password, 256);";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database connection error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        private bool RegisterUser(string username, string password, string initial, string accessLevel)
        {
            string connectionString = $"Server={config["Server"]};Database={config["Database"]};User ID={config["User"]};Password={config["Password"]};";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM sys_login_credentials WHERE username = @username;";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose another one.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return false;
                        }
                    }

                    // Insert new user with initial and access level
                    string query = "INSERT INTO sys_login_credentials (username, password, initial, accesslevel) VALUES (@username, SHA2(@password, 256), @initial, @accessLevel);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@initial", initial);
                        cmd.Parameters.AddWithValue("@accessLevel", accessLevel);
                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving account: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isCreatingAccount)
            {
                // Assign default values for new users
                string initial = username.Substring(0, 2).ToUpper(); // Example: "John" → "JO"
                string accessLevel = "0"; // Default role

                // Register the user
                if (RegisterUser(username, password, initial, accessLevel))
                {
                    MessageBox.Show("Account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Switch back to login mode
                    Dispatcher.Invoke(() =>
                    {
                        btnLogin.Content = "LOGIN";
                        isCreatingAccount = !isCreatingAccount;
                        if (btnCreate.Template != null)
                        {
                            TextBlock buttonText = btnCreate.Template.FindName("buttonText", btnCreate) as TextBlock;
                            if (buttonText != null)
                            {
                                buttonText.Text = isCreatingAccount ? "Login" : "Create Account";
                            }
                        }
                        isCreatingAccount = false;
                    });
                }
            }
            else
            {
                // Authenticate user
                if (AuthenticateUser(username, password))
                {
                    IsAuthenticated = true;
                    MessageBox.Show("Login Successful!", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Save "Remember Me" settings
                    config["RememberMe"] = checkBoxRememberMe.IsChecked == true ? "true" : "false";
                    if (config["RememberMe"] == "true")
                        config["SavedUsername"] = username;
                    else
                        config.Remove("SavedUsername");

                    ConfigHelper.SaveConfig(config);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            isCreatingAccount = !isCreatingAccount;

            // Update the text of the TextBlock inside the ControlTemplate
            if (btnCreate.Template != null)
            {
                TextBlock buttonText = btnCreate.Template.FindName("buttonText", btnCreate) as TextBlock;
                if (buttonText != null)
                {
                    buttonText.Text = isCreatingAccount ? "Login" : "Create Account";
                }
            }

            btnLogin.Content = isCreatingAccount ? "Create Account" : "LOGIN";
        }
    }
}
