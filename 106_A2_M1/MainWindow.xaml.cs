using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _106_A2_M1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Perform authentication logic (for example, check credentials against a database)
            if (IsValidUser(username, password))
            {
                MessageBox.Show("Login successful!");
                // Navigate to the next window or perform other actions after successful login
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
                // Clear input fields or perform other actions after unsuccessful login attempt
            }
        }

        private bool IsValidUser(string username, string password)
        {
            // Replace this with your authentication logic (e.g., check credentials against a database)
            // For example:
            // return YourAuthenticationService.ValidateUser(username, password);
            return username == "admin" && password == "password";
        }
    }
}
