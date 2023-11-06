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
    /// Interaction logic for LoginFrameView.xaml
    /// </summary>
    public partial class LoginFrameView : UserControl
    {
        public LoginFrameView()
        {
            InitializeComponent();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            // Traverse the visual tree to find the Frame with the name "LoginFrame"
            Frame LoginFrame = FindParent<Frame>(this, "LoginFrame");

            if (LoginFrame != null)
            {
                // Now you can navigate within the Frame
                LoginFrame.Navigate(new CreateAccountFrameView());
            }
            else
            {
                // Handle the case where the Frame is not found
                MessageBox.Show("LoginFrame not found.");
            }
        }

        private T FindParent<T>(DependencyObject child, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is T frameworkElement && frameworkElement.Name == name)
                {
                    return frameworkElement;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            // Perform authentication logic (for example, check credentials against a database)
            if (IsValidUser(email, password))
            {
                // Navigate to the next window or perform other actions after successful login
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                Frame mainFrame = mainWindow.FindName("MainFrame") as Frame;

                AdminDashboard adminDashboard = new AdminDashboard();
                mainFrame.Content = adminDashboard;

            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
                // Clear input fields or perform other actions after unsuccessful login attempt
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Simulate Login button click when pressing enter in password box
            if(e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
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
