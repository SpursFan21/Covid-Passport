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
using System.Threading;
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
        public ICommand CreateAccountCommand { get; set; }

        private BaseUser _baseUser;
        // ViewModel property to access UserDB data
        private UserDB _userDbData; 
        public UserDB UserDbData 
        {     
            get { return _userDbData; }     
            set    { _userDbData = value;         
                OnPropertyChanged(nameof(UserDbData)); // Notify property changed
            }
        
        }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        private DateTime _userDOB = new DateTime(2000,1,1);
        public DateTime UserDOB
        {
            get => _userDOB;
            set
            {
                if(_userDOB != value)
                {
                    _userDOB = value;
                    OnPropertyChanged(nameof(UserDOB));
                }
            }
        }
        public string UserEmail { get; set; }
        public string UserNHI { get; set; }
        public string UserPassword { private get; set; }
        public string UserPassword2 { private get; set; }


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
            UserDbData = new UserDB();
            // Set the default frame to WelcomeBackFrame
            CurrentDisplayFrame = new WelcomeBackFrame();
            CurrentDisplayFrame.DataContext = this;

            NavAdminDashboardCommand = new RelayCommand(async x => await PerformAdminLogin());
            NavUserDashboardCommand = new RelayCommand(async x => await PerformUserLogin());
            NavCreateAccountCommand = new RelayCommand(x => NavigateToFrame(new CreateAccountFrame()));
            WelcomeBackCommand = new RelayCommand(x => NavigateToFrame(new WelcomeBackFrame()));
            LoginCommand = new RelayCommand(async x => await PerformLogin());
            CreateAccountCommand = new RelayCommand(x => PerformCreateAccount());
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
            await _baseUser.GetLoginAsync(LoginEmail, LoginPassword);
            SetUserType();
            
        }

        public async Task PerformAdminLogin()
        {
            await _baseUser.GetLoginAsync("admin@admin.com", "gaming");
            SetUserType();

        }

        public async Task PerformUserLogin()
        {
            await _baseUser.GetLoginAsync("test@me.com", "gamer");
            SetUserType();

        }
        public async void SetUserType()
        {
            if (_baseUser.UserType == 1)
            {
                // Nav command to Admin Dashboard
                NavigateToPage(new AdminDashboardPage());
            }
            else if (_baseUser.UserType == 2)
            {
                // Nav command to User Dashboard
                UserDbData = await GetUserDataAsync();
                // Thread.Sleep(TimeSpan.FromSeconds(3)); // Delay for 3 seconds
                NavigateToPage(new UserDashboardPage(UserDbData));
            }
            else
            {
                ShowErrorPopup("Login Failed.\nEmail or password is incorrect.");
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


        private async Task<UserDB> GetUserDataAsync()
        {
            UserDbData = await _baseUser.RetrieveUserInformationAsync();
            return UserDbData;
        }

        private async void PerformCreateAccount()
        {
            try
            {
                if(UserPassword != UserPassword2)
                {
                    throw new ArgumentException("Passwords do not match.\nPlease try again.");
                }
                
                // Call create account model function
                await _baseUser.CreateAccountAsync(UserEmail, UserPassword, UserFirstName, UserLastName, UserDOB, UserNHI);
                ShowSuccessPopup("Welcome " + UserFirstName + "! You're account has been made!");
                NavigateToFrame(new WelcomeBackFrame());
            }

            catch (Exception ex)
            {
                ShowErrorPopup(ex.Message);
            }
        }

    }
}
