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

namespace _106_A2_M1.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginTemplate : UserControl
    {
        public LoginTemplate()
        {
            InitializeComponent();

        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            // Perform authentication logic (for example, check credentials against a database)
            if (IsValidUser(email, password))
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

        private bool IsValidUser(string email, string password)
        {
            // Replace this with your authentication logic (e.g., check credentials against a database)
            // For example:
            // return YourAuthenticationService.ValidateUser(username, password);
            return email == "admin" && password == "password";
        }
    }
}
