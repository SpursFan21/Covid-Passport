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
        private UserDB _userData; // Declare an instance of the UserDB class MODEL to ViewModel Pipeline
        
        public UserDB User_Data
        {
            get => _userData;

            set
            {
                if(_userData != value)
                {
                    _userData = value;
                    OnPropertyChanged(nameof(User_Data));
                }
            }
        }
        private BaseUser _baseUser;

        
        private int qrnum;
        private UserControl _qrUserControl;

        public UserControl QRUserControl
        {
            get => _qrUserControl;
            set
            {
                if (_qrUserControl != value)
                {
                    _qrUserControl = value;
                    OnPropertyChanged(nameof(QRUserControl));
                }
            }
        }
        
        public string FirstName
        {
            get => User_Data.first_name;
            set
            {
                if (User_Data.first_name != value)
                {
                    User_Data.first_name = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => User_Data.last_name;
            set
            {
                if (User_Data.last_name != value)
                {
                    User_Data.last_name = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public int UserDOB
        {
            get => User_Data.dob;
            set
            {
                if (User_Data.dob != value)
                {
                    User_Data.dob = value;
                    OnPropertyChanged(nameof(UserDOB));
                }
            }
        }

        public string UserEmail
        {
            get => User_Data.email;
            set
            {
                if (User_Data.email != value)
                {
                    User_Data.email = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }

        public string UserNHI
        {
            get => User_Data.nhi_num;
            set
            {
                if (User_Data.nhi_num != value)
                {
                    User_Data.nhi_num = value;
                    OnPropertyChanged(nameof(UserNHI));
                }
            }
        }
        
        public int QRStatus
        {
            get => User_Data.qr_status;
            set
            {
                if(User_Data.qr_status != value)
                {
                    User_Data.qr_status = value;
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
        public UserDashboardViewModel(UserDB _uDB)
        {
            // Initialize MODEL instances to ViewModel Pipeline
            _user = new User();
            _userData = new UserDB();
            _userData = _uDB;
            _baseUser = new BaseUser();

            qrnum = 2; // TESTING VARIABLE
            //InitializeAsync(); // Loads userDb into this instance

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
            User_Data = await BaseUser.RetrieveUserInformationAsync();
        }

        private void ShowQRFrame()
        {
            try
            {
                // Set display frame based on user QRStatus
                if (qrnum == 0)
                {
                    QRUserControl = new UserMyVaccinePassFrame_QR0();
                }
                else if (qrnum == 1)
                {
                    QRUserControl = new UserMyVaccinePassFrame_QR1();
                }
                else if (qrnum == 2)
                {
                    QRUserControl = new UserMyVaccinePassFrame_QR2();
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
