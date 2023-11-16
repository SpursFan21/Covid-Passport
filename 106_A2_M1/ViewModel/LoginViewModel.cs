using _106_A2_M1.Model;
using _106_A2_M1.Services;
using _106_A2_M1.View;
using _106_A2_M1.View.Pages;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _106_A2_M1.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand NavAdminDashboardCommand { get; set; }
        public ICommand NavUserDashboardCommand { get; set; }
        public ICommand NavCreateAccountCommand { get; set; }
        public ICommand WelcomeBackCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        private BaseUser _baseUser;

        public string _loginEmail;
        public string LoginEmail
        {
            get => _loginEmail;
            set
            {
                if (_loginEmail != value)
                {
                    _loginEmail = value;
                    OnPropertyChanged(nameof(LoginEmail));
                }
            }
        }
        public string LoginPassword { private get; set; }
        
        public LoginViewModel()
        {
            _baseUser = new BaseUser();

            // Set the default frame to WelcomeBackFrame
            CurrentDisplayFrame = new WelcomeBackFrame();
            CurrentDisplayFrame.DataContext = this;

            NavAdminDashboardCommand = new RelayCommand(x => NavigateToPage(new AdminDashboardPage()));
            NavUserDashboardCommand = new RelayCommand(x => NavigateToPage(new UserDashboardPage()));
            NavCreateAccountCommand = new RelayCommand(x => NavigateToFrame(new CreateAccountFrame()));
            WelcomeBackCommand = new RelayCommand(x => NavigateToFrame(new WelcomeBackFrame()));
            LoginCommand = new RelayCommand(async x => await PerformLogin());
        }

        private void NavigateToPage(Page destinationPage)
        {
            MainWindowVM mainVM = (MainWindowVM)Application.Current.MainWindow.DataContext;
            mainVM.CurrentDisplayPage = destinationPage;
        }

        private void NavigateToFrame(UserControl destinationFrame)
        {
            CurrentDisplayFrame = destinationFrame;
            CurrentDisplayFrame.DataContext = this;
        }

        public async Task PerformLogin()
        {
            MessageBox.Show("Password entered: " + LoginPassword);
            await _baseUser.GetLoginAsync(LoginEmail, LoginPassword);
            SetUserType();
        }

        public void SetUserType()
        {
            if (_baseUser.UserType == 1)
            {
                // Nav command to Admin Dashboard
                NavigateToPage(new AdminDashboardPage());
            }
            else if (_baseUser.UserType == 2)
            {
                // Nav command to User Dashboard
                NavigateToPage(new UserDashboardPage());
            }
            else
            {
                Console.WriteLine("Login Failed");
            }
        }

        // Property to check if the user is an Admin
        public bool IsAdmin
        {
            get { return _baseUser.UserType == 1; }
        }

        // Property to check if the user is a regular User
        public bool IsUser
        {
            get { return _baseUser.UserType == 2; }
        }


        /*private async Task PerformLogin()
        {
            string email = LoginEmail;
            string password = LoginPassword;
            //await _baseUser.Login(email, password);
        }*/
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Update the LoginPassword property when the password changes
            //LoginPassword = txtPassword.Password;
        }
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var passwordBox = (PasswordBox)sender;
                string password = passwordBox.Password;

                // Assuming you have an instance of your ViewModel (viewModel) or Model (model)
                // viewModel.HandlePassword(password); // Call a method in ViewModel
                // model.HandlePassword(password); // Call a method in Model
            }
        }


        
        public string Id
        {
            get { return _baseUser.id; }
            set
            {
                if (_baseUser.id != value)
                {
                    _baseUser.id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public int AccountType
        {
            get { return _baseUser.account_type; }
            set
            {
                if (_baseUser.account_type != value)
                {
                    _baseUser.account_type = value;
                    OnPropertyChanged(nameof(AccountType));
                }
            }
        }
        public string u_token
        {
            get { return _baseUser.u_token; }
            set
            {
                if (_baseUser.u_token != value)
                {
                    _baseUser.u_token = value;
                    OnPropertyChanged(nameof(_baseUser.u_token));
                }
            }
        }
        public string image_link
        {
            get { return _baseUser.image_link; }
            set
            {
                if (_baseUser.image_link != value)
                {
                    _baseUser.image_link = value;
                    OnPropertyChanged(nameof(_baseUser.image_link));
                }
            }
        }
    }
}
