using _106_A2_M1.Model;
using _106_A2_M1.Services;
using _106_A2_M1.View.Pages;
using _106_A2_M1.View.PopupWindows;
using _106_A2_M1.View.UserFrames;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace _106_A2_M1.ViewModel
{
    public class UserDashboardViewModel : ViewModelBase
    {
        // Navigation Commands
        public ICommand NavMyRecordsCommand { get; }
        public ICommand NavMyVaccinePassCommand { get; }
        public ICommand LogoutCommand { get; }

        // Test Result Commands
        public ICommand AddTestResultCommand { get; }
        public ICommand ShowPopupCommand { get; }
        public ICommand ClosePopupCommand { get; }

        // QR Code Commands
        public ICommand RequestQRCommand { get; }

        // User Member Variables
        public string UserFullName { get; private set; }
        
        
        private User _activeUser; // Declare an instance of the User class MODEL to ViewModel Pipeline
        public User ActiveUser
        {
            get => _activeUser;
            set
            {
                if(_activeUser != value)
                {
                    _activeUser = value;
                    OnPropertyChanged(nameof(ActiveUser));
                }
            }
        }

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
            get => ActiveUser.UserDB.first_name;
            set
            {
                if (ActiveUser.UserDB.first_name != value)
                {
                    ActiveUser.UserDB.first_name = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => ActiveUser.UserDB.last_name;
            set
            {
                if (ActiveUser.UserDB.last_name != value)
                {
                    ActiveUser.UserDB.last_name = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public int UserDOB
        {
            get => ActiveUser.UserDB.dob;
            set
            {
                if (ActiveUser.UserDB.dob != value)
                {
                    ActiveUser.UserDB.dob = value;
                    OnPropertyChanged(nameof(UserDOB));
                }
            }
        }

        public string UserEmail
        {
            get => ActiveUser.UserDB.email;
            set
            {
                if (ActiveUser.UserDB.email != value)
                {
                    ActiveUser.UserDB.email = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }

        public string UserNHI
        {
            get => ActiveUser.UserDB.nhi_num;
            set
            {
                if (ActiveUser.UserDB.nhi_num != value)
                {
                    ActiveUser.UserDB.nhi_num = value;
                    OnPropertyChanged(nameof(UserNHI));
                }
            }
        }

        public int QRStatus
        {
            get => ActiveUser.UserDB.qr_status;
            set
            {
                if (ActiveUser.UserDB.qr_status != value)
                {
                    ActiveUser.UserDB.qr_status = value;
                    OnPropertyChanged(nameof(QRStatus));
                }
            }
        }

        public int VaccineStatus
        {
            get => ActiveUser.UserDB.vaccine_status;
            set
            {
                if (ActiveUser.UserDB.vaccine_status != value)
                {
                    ActiveUser.UserDB.vaccine_status = value;
                    OnPropertyChanged(nameof(VaccineStatus));
                }
            }
        }

        public string QRImageURL
        {
            get => ActiveUser.storedQRCodeImageURL;
            set
            {
                if(ActiveUser.storedQRCodeImageURL != value)
                {
                    ActiveUser.storedQRCodeImageURL = value;
                    OnPropertyChanged(nameof(QRImageURL));
                }
            }
        }

        public string ExpDate
        {
            get => ActiveUser.UrlExpDate;
            set
            {
                if(ActiveUser.UrlExpDate != value)
                {
                    ActiveUser.UrlExpDate = value;
                    OnPropertyChanged(nameof(ExpDate));
                }
            }
        }

        // Test Result Popup
        private AddTestResultPopup _addTestResultPopup;

        private bool _isPopupOpen;
        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
            set
            {
                if (_isPopupOpen != value)
                {
                    _isPopupOpen = value;
                    OnPropertyChanged(nameof(IsPopupOpen));
                    OnPropertyChanged(nameof(DarkenedBackgroundVisibility));
                }
            }
        }

        public Visibility DarkenedBackgroundVisibility
        {
            get { return IsPopupOpen ? Visibility.Visible : Visibility.Collapsed; }
        }

        private bool _isPositiveSelected;
        public bool IsPositiveSelected
        {
            get { return _isPositiveSelected; }
            set
            {
                if (_isPositiveSelected != value)
                {
                    _isPositiveSelected = value;
                    OnPropertyChanged(nameof(IsPositiveSelected));
                }
            }
        }

        private bool _isNegativeSelected;
        public bool IsNegativeSelected
        {
            get { return _isNegativeSelected; }
            set
            {
                if (_isNegativeSelected != value)
                {
                    _isNegativeSelected = value;
                    OnPropertyChanged(nameof(IsNegativeSelected));
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
            _activeUser = new User();
            _activeUser.UserDB = _uDB;
            UpdateUserFullName();

            //InitializeAsync(); // Loads userDb into this instance

            // Startup display for user login
            FrameTitle = "My Vaccine Pass";
            InitializeQRAsync();
            NavigateToFrame(new UserMyVaccinePassControlFrame());
            ShowQRFrame();

            _addTestResultPopup = new AddTestResultPopup();
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
                InitializeQRAsync();
                NavigateToFrame(new UserMyVaccinePassControlFrame());
                ShowQRFrame();
            });

            // Feature Commands
            RequestQRCommand = new RelayCommand(x => RequestQRCode());
            AddTestResultCommand = new RelayCommand(x => AddTestResult());
            ShowPopupCommand = new RelayCommand(x => ShowPopup());
            ClosePopupCommand = new RelayCommand(x => ClosePopup());

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

        // TESTING PURPOSES ONLY
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
            UserFullName = $"{FirstName} {LastName}";
            OnPropertyChanged(nameof(UserFullName));
        }

        private async void InitializeAsync()
        {
            await GetUserDataAsync();
        }

        private async Task GetUserDataAsync()
        {
            ActiveUser.UserDB = await ActiveUser.RetrieveUserInformationAsync();
        }

        private async void ShowQRFrame()
        {
            try
            {
                // Set display frame based on user QRStatus
                if (QRStatus == 0)
                {
                    QRUserControl = new UserMyVaccinePassFrame_QR0();
                }
                else if (QRStatus == 1)
                {
                    QRUserControl = new UserMyVaccinePassFrame_QR1();
                }
                else if (QRStatus == 2)
                {
                    await Task.Delay(300);
                    QRUserControl = new UserMyVaccinePassFrame_QR2();
                }
                else
                {
                    // Handle unexpected values
                    throw new InvalidOperationException($"Unexpected value of QRStatus: {QRStatus}");
                }
            }
            catch (Exception ex)
            {
                // Show error message as a popup
                ShowErrorPopup($"An error occurred in ShowQRFrame: {ex.Message}");
            }
        }

        private void RequestQRCode()
        {
            // Update QR status
        }

        private void AddTestResult()
        {
            try
            {
                if(!IsPositiveSelected && !IsNegativeSelected)
                {
                    throw new ArgumentException("Please select result.");
                }
                if (IsPositiveSelected)
                {
                    ClosePopup();
                }
                else if (IsNegativeSelected)
                {
                    ClosePopup();
                }
            }
            catch (Exception ex)
            {
                ShowErrorPopup(ex.Message);
            }
        }

        private async Task InitializeQRAsync()
        {
            await ActiveUser.GetQRData();
        }

        private void ShowPopup()
        {
            // Show the popup here
            _addTestResultPopup = new AddTestResultPopup();
            IsPopupOpen = true;
        }
        private void ClosePopup()
        {
            if (IsPopupOpen)
            {
                IsPopupOpen = false;
            }
        }
    }
}
