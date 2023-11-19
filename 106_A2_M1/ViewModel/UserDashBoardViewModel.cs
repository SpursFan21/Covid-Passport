using _106_A2_M1.Model;
using _106_A2_M1.Services;
using _106_A2_M1.View.Pages;
using _106_A2_M1.View.UserFrames;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public string UserFullName { get; private set; }
        private User _user; // Declare an instance of the User class MODEL to ViewModel Pipeline
        private UserDB _userDB; // Declare an instance of the UserDB class MODEL to ViewModel Pipeline
        private Dictionary<string, object> _userData;

        public Dictionary<string, object> UserData
        {
            get => _userData;
            set
            {
                if(_userData != value)
                {
                    _userData = value;
                    OnPropertyChanged(nameof(UserData));
                }
            }
        }
        /*
        public string FirstName
        {
            get => User.UserDB.first_name;
            set
            {
                if (User.UserDB.first_name != value)
                {
                    User.UserDB.first_name = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => User.UserDB.last_name;
            set
            {
                if (User.UserDB.last_name != value)
                {
                    User.UserDB.last_name = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string UserDOB
        {
            get => User.UserDB.dob;
            set
            {
                if (User.UserDB.dob != value)
                {
                    User.UserDB.dob = value;
                    OnPropertyChanged(nameof(UserDOB));
                }
            }
        }

        public string UserEmail
        {
            get => User.UserDB.email;
            set
            {
                if (User.UserDB.email != value)
                {
                    User.UserDB.email = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }

        public int UserNHI
        {
            get => User.UserDB.nhi_num;
            set
            {
                if (User.UserDB.nhi_num != value)
                {
                    User.UserDB.nhi_num = value;
                    OnPropertyChanged(nameof(UserNHI));
                }
            }
        }

        public int QRStatus
        {
            get => User.UserDB.qr_status;
            set
            {
                if(User.UserDB.qr_status != value)
                {
                    User.UserDB.qr_status = value;
                    OnPropertyChanged(nameof(QRStatus));
                }
            }
        }

    */

        private ObservableCollection<CovidTest> _testList = new ObservableCollection<CovidTest>();
        public ObservableCollection<CovidTest> TestList
        {
            get => _testList;
            set
            {
                _testList = value;
                OnPropertyChanged(nameof(TestList));
            }
        }
        public UserDashboardViewModel()
        {
            _user = new User(); // Initialize a new User instance MODEL to ViewModel Pipeline
            _userDB = new UserDB(); // Might not need these

            UserData = _GetUserData(); // Loads userDb into this instance
            /*
            int id = (int)UserData[nameof(id)];
            string email = (string)UserData[nameof(email)];
            string first_name = (string)UserData[nameof(first_name)];
            string last_name = (string)UserData[nameof(last_name)];
            DateTime dob = (DateTime)UserData[nameof(dob)];
            string nhi_num = (string)UserData[nameof(nhi_num)];
            bool qr_status = (bool)UserData[nameof(qr_status)];
            int issue_ct = (int)UserData[nameof(issue_ct)];
            int test_ct = (int)UserData[nameof(test_ct)];
            string vaccine_status = (string)UserData[nameof(vaccine_status)];
            UpdateUserFullName(); 
            */

            FrameTitle = "My Vaccine Pass";
            NavigateToFrame(new UserMyVaccinePassFrame_QR0());

            // Navigation commands
            LogoutCommand = new RelayCommand(x => NavigateToPage(new LoginPage()));
            NavMyRecordsCommand = new RelayCommand(x =>
            {
                FrameTitle = "My Records";
                NavigateToFrame(new UserMyRecordsFrame());
            });
            NavMyVaccinePassCommand = new RelayCommand(x =>
            {
                FrameTitle = "My Vaccine Pass";
                NavigateToFrame(new UserMyVaccinePassFrame_QR0());
            });
            
            // Test list for TESTING PURPOSES ONLY
            TestList = new ObservableCollection<CovidTest>();
            generateTest(10102023, false, "RAT");
            generateTest(02022023, true, "PCR");
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
        private void generateTest(int date, bool result, string type)
        {
            CovidTest test = new CovidTest();
            test.test_date = date;
            test.result = result;
            test.test_type = type;
            TestList.Add(test);
        }

        private void UpdateUserFullName()
        {
            //UserFullName = $"{FirstName} {LastName}";
            //OnPropertyChanged(nameof(UserFullName));
        }

        private Dictionary<string, object> _GetUserData()
        {
            return _userDB.GetUserData();
        }
    }
}
