using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginScreen;

namespace NoLandholdingApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (loginWindow.IsAuthenticated)  // Check property directly
            {
                Application.Run(new formInput());
            }
            Console.WriteLine($"LoginWindow closed. IsAuthenticated = {loginWindow.IsAuthenticated}");
        }
    }
}
