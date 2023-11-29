using _106_A2_M1.ViewModel;
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
    /// Interaction logic for WelcomeBackFrame.xaml
    /// </summary>
    public partial class WelcomeBackFrame : UserControl
    {
        public WelcomeBackFrame()
        {
            InitializeComponent();

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).LoginPassword = ((PasswordBox)sender).Password; }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = DataContext as LoginViewModel;
                viewModel?.LoginCommand.Execute(null);
            }
        }

        /*
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
       LoginFrame.Navigate(new CreateAccountFrame());
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

       AdminDashboardView adminDashboard = new AdminDashboardView();
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
}*/
    }
}
