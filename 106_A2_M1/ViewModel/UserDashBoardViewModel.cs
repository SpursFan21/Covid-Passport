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
        private BaseUser _baseUser;
        private Dictionary<string, object> _userData;

        private int qrnum;
        private UserControl _selectedUserControl;

        public UserControl SelectedUserControl
        {
            get { return _selectedUserControl; }
            set
            {
                if (_selectedUserControl != value)
                {
                    _selectedUserControl = value;
                    OnPropertyChanged(nameof(SelectedUserControl));
                }
            }
        }

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
        
        public string FirstName
        {
            get => _userDB.first_name;
            set
            {
                if (_userDB.first_name != value)
                {
                    _userDB.first_name = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => _userDB.last_name;
            set
            {
                if (_userDB.last_name != value)
                {
                    _userDB.last_name = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public int UserDOB
        {
            get => _userDB.dob;
            set
            {
                if (_userDB.dob != value)
                {
                    _userDB.dob = value;
                    OnPropertyChanged(nameof(UserDOB));
                }
            }
        }

        public string UserEmail
        {
            get => _userDB.email;
            set
            {
                if (_userDB.email != value)
                {
                    _userDB.email = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }

        public string UserNHI
        {
            get => _userDB.nhi_num;
            set
            {
                if (_userDB.nhi_num != value)
                {
                    _userDB.nhi_num = value;
                    OnPropertyChanged(nameof(UserNHI));
                }
            }
        }

        public int QRStatus
        {
            get => _userDB.qr_status;
            set
            {
                if(_userDB.qr_status != value)
                {
                    _userDB.qr_status = value;
                    OnPropertyChanged(nameof(QRStatus));
                }
            }
        }

    

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
            // Initialize MODEL instances to ViewModel Pipeline
            _user = new User(); 
            _userDB = new UserDB();
            _baseUser = new BaseUser();

            qrnum = 1; // TESTING VARIABLE
            //InitializeAsync(); // Loads userDb into this instance
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

            // Startup display for user login
            FrameTitle = "My Vaccine Pass";
            NavigateToFrame(new UserMyVaccinePassControlFrame());
            ShowQRFrame();

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
                NavigateToFrame(new UserMyVaccinePassControlFrame());
                ShowQRFrame();
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

        private async void InitializeAsync()
        {
            await GetUserDataAsync();
        }

        private async Task GetUserDataAsync()
        {
            _userDB = await _baseUser.RetrieveUserInformationAsync();
        }

        private void ShowQRFrame()
        {
            try
            {
                // Set display frame based on user QRStatus
                if (qrnum == 0)
                {
                    SelectedUserControl = new UserMyVaccinePassFrame_QR0();
                }
                else if (qrnum == 1)
                {
                    SelectedUserControl = new UserMyVaccinePassFrame_QR1();
                }
                else if (qrnum == 2)
                {
                    SelectedUserControl = new UserMyVaccinePassFrame_QR2();
                }
                else
                {
                    // Handle unexpected values
                    throw new InvalidOperationException($"Unexpected value of qrnum: {qrnum}");
                }
            }
            catch (Exception ex)
            {
                // Show error message as a popup
                ShowErrorPopup($"An error occurred in ShowQRFrame: {ex.Message}");
            }
        }
        
    }
}
