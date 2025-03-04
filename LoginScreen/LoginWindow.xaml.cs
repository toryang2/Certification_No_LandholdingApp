using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using CommonLibrary;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Input;

namespace LoginScreen
{
    public partial class LoginWindow : Window
    {
        private DispatcherTimer messageTimer;

        public static string CurrentUserInitials { get; private set; } = string.Empty;
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

            txtFullName.GotFocus += (s, e) => PlaceholderTextFullName.Visibility = Visibility.Collapsed;
            txtFullName.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                    PlaceholderTextFullName.Visibility = Visibility.Visible;
            };

            txtInitial.GotFocus += (s, e) => PlaceholderTextInitial.Visibility = Visibility.Collapsed;
            txtInitial.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtInitial.Text))
                    PlaceholderTextInitial.Visibility = Visibility.Visible;
            };

            txtFullName.Visibility = Visibility.Collapsed;
            PlaceholderTextFullName.Visibility = Visibility.Collapsed;
            recFullName.Visibility = Visibility.Collapsed;

            txtInitial.Visibility = Visibility.Collapsed;
            PlaceholderTextInitial.Visibility = Visibility.Collapsed;
            recInitial.Visibility = Visibility.Collapsed;

            // Load "Remember Me" feature
            if (config.ContainsKey("RememberMe") && config["RememberMe"] == "true")
            {
                txtUsername.Text = config.ContainsKey("SavedUsername") ? config["SavedUsername"] : "";
                checkBoxRememberMe.IsChecked = true;

                // 🚀 Immediately hide the placeholder if username is restored
                if (!string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    PlaceholderText.Visibility = Visibility.Collapsed;
                    txtPassword.Focus();
                }
            }

            // Press Enter to trigger login
            txtUsername.KeyDown += TxtBox_KeyDown;
            txtPassword.KeyDown += TxtBox_KeyDown;

            // Direct event wiring
            txtUsername.GotFocus += TextBox_GotFocus;
            txtInitial.GotFocus += TextBox_GotFocus;
            txtPassword.GotFocus += PasswordBox_GotFocus; // PasswordBox handled separately
            txtFullName.GotFocus += TextBox_GotFocus;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (WasTabPressedToReachControl())
                {
                    textBox.SelectAll();
                }
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (WasTabPressedToReachControl())
                {
                    passwordBox.Dispatcher.BeginInvoke(new Action(() => passwordBox.SelectAll()));
                }
            }
        }

        // Helper to detect if focus came from keyboard (Tab key)
        private bool WasTabPressedToReachControl()
        {
            // Check if the Tab key is down, or if it's a Shift+Tab (reverse tabbing)
            return Keyboard.IsKeyDown(Key.Tab) ||
                   (Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.Tab)) ||
                   (Keyboard.IsKeyDown(Key.RightShift) && Keyboard.IsKeyDown(Key.Tab));
        }

        private void TxtBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnLogin_Click(btnLogin, null);
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

        private bool RegisterUser(string username, string password, string initial, string accessLevel, string fullname)
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
                    string query = "INSERT INTO sys_login_credentials (username, password, initial, accesslevel, name) VALUES (@username, SHA2(@password, 256), @initial, @accessLevel, @fullname);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@initial", initial.ToUpper());
                        cmd.Parameters.AddWithValue("@accessLevel", accessLevel);
                        cmd.Parameters.AddWithValue("@fullname", fullname.ToUpper());
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

        private void ShowMessage(string message, Brush color = null)
        {
            lblMessage.Content = message;
            lblMessage.Foreground = color ?? Brushes.Black;

            lblMessage.Visibility = Visibility.Visible;

            // Start a 3-second timer to clear the message
            if (messageTimer == null)
            {
                messageTimer = new DispatcherTimer();
                messageTimer.Interval = TimeSpan.FromSeconds(3);
                messageTimer.Tick += (s, e) =>
                {
                    lblMessage.Content = "";
                    messageTimer.Stop(); // Stop the timer after clearing the message
                };
            }

            messageTimer.Stop(); // Reset timer if message changes before 3 seconds is up
            messageTimer.Start();
        }


        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            // Clear previous message
            lblMessage.Content = "";
            lblMessage.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Please enter both username and password.", Brushes.Red);

                if (string.IsNullOrEmpty(username))
                {
                    txtUsername.Focus();
                    txtUsername.SelectAll();
                }
                else
                {
                    txtPassword.Focus();
                    txtPassword.SelectAll();
                }

                return;
            }


            if (isCreatingAccount)
            {
                string fullName = txtFullName.Text.Trim();
                if (string.IsNullOrEmpty(fullName))
                {
                    ShowMessage("Please enter your full name.", Brushes.Red);
                    return;
                }

                string initial = txtInitial.Text.Trim();
                string accessLevel = "0"; // Default role

                if ( username.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    accessLevel = "1"; // Admin role
                }

                if (RegisterUser(username, password, initial, accessLevel, fullName))
                {
                    ShowMessage("Account created successfully!", Brushes.Green);

                    // Switch back to login mode with animation
                    SwitchToLoginModeWithAnimation();
                }
            }
            else
            {
                if (AuthenticateUser(username, password))
                {
                    IsAuthenticated = true;
                    ShowMessage("Login Successful!", Brushes.Green);

                    // ✅ Save to UserSession right after successful login
                    UserSession.Username = username;
                    UserSession.Initials = UserRepository.GetInitialsForUser(username);
                    UserSession.AccessLevel = UserRepository.GetAccessLevelForUser(username);

                    // Save "Remember Me" settings
                    config["RememberMe"] = checkBoxRememberMe.IsChecked == true ? "true" : "false";
                    if (config["RememberMe"] == "true")
                        config["SavedUsername"] = username;
                    else
                        config.Remove("SavedUsername");

                    ConfigHelper.SaveConfig(config);

                    await Task.Delay(2000);

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    ShowMessage("Invalid credentials. Please try again.", Brushes.Red);
                    txtUsername.Focus();
                    txtUsername.SelectAll();
                }
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            isCreatingAccount = !isCreatingAccount;

            // Update button texts
            if (btnCreate.Template != null)
            {
                TextBlock buttonText = btnCreate.Template.FindName("buttonText", btnCreate) as TextBlock;
                if (buttonText != null)
                {
                    buttonText.Text = isCreatingAccount ? "Login" : "Create Account";
                }
            }
            btnLogin.Content = isCreatingAccount ? "Create Account" : "LOGIN";

            // Full Name + Initial Fields
            if (isCreatingAccount)
            {
                ShowFullNameFieldsWithAnimation();
                HideRememberMeWithAnimation();
                txtFullName.Text = string.Empty;
                txtInitial.Text = string.Empty;
            }
            else
            {
                HideFullNameFieldsWithAnimation();
                ShowRememberMeWithAnimation();
            }

            // Animate Login Button position
            double to = isCreatingAccount ? 510 : 433;
            ThicknessAnimation buttonMoveAnim = new ThicknessAnimation
            {
                To = new Thickness(198, to, 0, 0),
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            btnLogin.BeginAnimation(MarginProperty, buttonMoveAnim);
        }

        private void ShowFullNameFieldsWithAnimation()
        {
            SetFullNameFieldsVisibility(Visibility.Visible);
            SetFullNameFieldsHitTest(true);

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            DoubleAnimation recFadeIn = new DoubleAnimation
            {
                To = 0.8,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            txtFullName.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            PlaceholderTextFullName.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            txtInitial.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            PlaceholderTextInitial.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            recFullName.BeginAnimation(UIElement.OpacityProperty, recFadeIn);
            recInitial.BeginAnimation(UIElement.OpacityProperty, recFadeIn);
        }

        private void HideFullNameFieldsWithAnimation()
        {
            SetFullNameFieldsHitTest(false);  // Disable clicks right away

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            fadeOut.Completed += (s, e) =>
            {
                SetFullNameFieldsVisibility(Visibility.Collapsed);
            };

            txtFullName.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            PlaceholderTextFullName.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            txtInitial.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            PlaceholderTextInitial.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            recFullName.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            recInitial.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void ShowRememberMeWithAnimation()
        {
            checkBoxRememberMe.Visibility = Visibility.Visible;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            checkBoxRememberMe.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void HideRememberMeWithAnimation()
        {
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            fadeOut.Completed += (s, e) => checkBoxRememberMe.Visibility = Visibility.Collapsed;
            checkBoxRememberMe.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void SetFullNameFieldsVisibility(Visibility visibility)
        {
            txtFullName.Visibility = visibility;
            PlaceholderTextFullName.Visibility = visibility;
            recFullName.Visibility = visibility;
            txtInitial.Visibility = visibility;
            PlaceholderTextInitial.Visibility = visibility;
            recInitial.Visibility = visibility;
        }

        private void SetFullNameFieldsHitTest(bool enabled)
        {
            txtFullName.IsHitTestVisible = enabled;
            txtInitial.IsHitTestVisible = enabled;
        }


        private void SwitchToLoginModeWithAnimation()
        {
            isCreatingAccount = false;

            // Update button texts
            if (btnCreate.Template != null)
            {
                TextBlock buttonText = btnCreate.Template.FindName("buttonText", btnCreate) as TextBlock;
                if (buttonText != null)
                {
                    buttonText.Text = "Create Account";
                }
            }
            btnLogin.Content = "LOGIN";

            // Clear full name and initial fields to reset after account creation
            txtFullName.Text = string.Empty;
            txtInitial.Text = string.Empty;

            // Hide full name + initial fields with animation
            HideFullNameFieldsWithAnimation();

            // Show "Remember Me" checkbox with animation
            ShowRememberMeWithAnimation();

            // Move login button back up
            ThicknessAnimation buttonMoveAnim = new ThicknessAnimation
            {
                To = new Thickness(198, 433, 0, 0),
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            btnLogin.BeginAnimation(MarginProperty, buttonMoveAnim);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button?.Template.FindName("border", button) is Border border)
            {
                border.Background = new SolidColorBrush(Color.FromArgb(0, 221, 221, 221)); // #00DDDDDD
            }

            if (button?.Template.FindName("contentPresenter", button) is TextBlock textBlock)
            {
                textBlock.Foreground = Brushes.Black;
            }

            Environment.Exit(0);
        }
    }
}
