using _106_A2_M1.Model;
using _106_A2_M1.Services;
using _106_A2_M1.View.Pages;
using _106_A2_M1.View.UserFrames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _106_A2_M1.ViewModel
{
    public class UserDashboardViewModel : ViewModelBase
    {
        public ICommand NavMyRecordsCommand { get; }
        public ICommand NavMyVaccinePassCommand { get; }
        public ICommand AddTestResultCommand { get; }
        public ICommand LogoutCommand { get; }


        private User _user; // Declare an instance of the User class MODEL to ViewModel Pipeline

        private string _userFullName;
        public string UserFullName
        {
            get => _userFullName;
            set
            {
                if (_userFullName != value)
                {
                    _userFullName = value;
                    OnPropertyChanged(nameof(UserFullName));
                }
            }
        }
        public UserDashboardViewModel()
        {
            _user = new User("Hank", "Schrader", "pass"); // Initialize a new User instance MODEL to ViewModel Pipeline
            UserFullName = _user.db_member.first_name +" " + _user.db_member.last_name;
            FrameTitle = "My Vaccine Pass";
            NavigateToFrame(new UserMyVaccinePassFrame());

            LogoutCommand = new RelayCommand(x => NavigateToPage(new LoginPage()));
            NavMyRecordsCommand = new RelayCommand(x =>
            {
                FrameTitle = "My Records";
                NavigateToFrame(new UserMyRecordsFrame());
            });
            NavMyVaccinePassCommand = new RelayCommand(x =>
            {
                FrameTitle = "My Vaccine Pass";
                NavigateToFrame(new UserMyVaccinePassFrame());
            });
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
    }
}
