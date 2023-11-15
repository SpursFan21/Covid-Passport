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
        private BaseUser _user;
        public LoginViewModel()
        {
            _user = new BaseUser();

            // Set the default frame to WelcomeBackFrame
            CurrentDisplayFrame = new WelcomeBackFrame();
            CurrentDisplayFrame.DataContext = this;

            AdminDashboardCommand = new RelayCommand(x => NavigateToPage(new AdminDashboardPage()));
            NewAccountCommand = new RelayCommand(x => NavigateToFrame(new CreateAccountFrame()));
            WelcomeBackCommand = new RelayCommand(x => NavigateToFrame(new WelcomeBackFrame()));
        }
        
        public ICommand AdminDashboardCommand { get; set; }
        public ICommand NewAccountCommand { get; set; }
        public ICommand WelcomeBackCommand { get; set; }

        private UserControl _currentDisplayFrame;
        public UserControl CurrentDisplayFrame
        {
            get
            {
                return _currentDisplayFrame;
            }

            set
            {
                _currentDisplayFrame = value;
                OnPropertyChanged("CurrentDisplayFrame");
            }
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


        //public string Email => _user.db_member.email;
        //public string Password;

        public string Id
        {
            get { return _user.id; }
            set
            {
                if (_user.id != value)
                {
                    _user.id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public int AccountType
        {
            get { return _user.account_type; }
            set
            {
                if (_user.account_type != value)
                {
                    _user.account_type = value;
                    OnPropertyChanged(nameof(AccountType));
                }
            }
        }
        public string u_token
        {
            get { return _user.u_token; }
            set
            {
                if (_user.u_token != value)
                {
                    _user.u_token = value;
                    OnPropertyChanged(nameof(_user.u_token));
                }
            }
        }
        public string image_link
        {
            get { return _user.image_link; }
            set
            {
                if (_user.image_link != value)
                {
                    _user.image_link = value;
                    OnPropertyChanged(nameof(_user.image_link));
                }
            }
        }
    }
}
