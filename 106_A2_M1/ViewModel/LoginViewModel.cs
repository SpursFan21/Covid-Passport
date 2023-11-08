using _106_A2_M1.Model;
using _106_A2_M1.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace _106_A2_M1.ViewModel
{
    public class LoginViewModel : MainViewModel
    {
        private Model.BaseUser _user;

        public LoginViewModel()
        {
            _user = new BaseUser();
            LoginView loginView = new LoginView();
            loginView.DataContext = this;
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


        public string Email => _user.db_member.email;
        public string Password;

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
