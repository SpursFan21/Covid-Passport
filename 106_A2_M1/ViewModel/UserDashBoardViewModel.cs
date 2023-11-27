using _106_A2_M1.Model;
using _106_A2_M1.Services;
using _106_A2_M1.View.Pages;
using _106_A2_M1.View.PopupWindows;
using _106_A2_M1.View.UserFrames;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
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

        public string UserDOB
        {
            get => ActiveUser.UserDB.FormattedDOB;
            set
            {
                if (ActiveUser.UserDB.FormattedDOB != value)
                {
                    ActiveUser.UserDB.FormattedDOB = value;
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
        private UserControl _popupContent;
        public UserControl PopupContent
        {
            get { return _popupContent; }
            set
            {
                if (_popupContent != value)
                {
                    _popupContent = value;
                    OnPropertyChanged(nameof(PopupContent));
                }
            }
        }


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

        private string _selectedTestType;
        public string SelectedTestType
        {
            get { return _selectedTestType; }
            set
            {
                if (_selectedTestType != value)
                {
                    _selectedTestType = value;
                    OnPropertyChanged(nameof(SelectedTestType));
                }
            }
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    OnPropertyChanged(nameof(FormattedSelectedDate));
                }
            }
        }

        // Add a formatted string property
        public string FormattedSelectedDate => SelectedDate.ToString("dd-MM-yyyy");

        // Vaccine Doses
        public Vaccine FirstDose
        {
            get => ActiveUser.first_dose;
            set
            {
                if(ActiveUser.first_dose != null)
                {
                    ActiveUser.first_dose = value;
                    OnPropertyChanged(nameof(FirstDose));
                }
            }
        }

        public Vaccine SecondDose
        {
            get => ActiveUser.second_dose;
            set
            {
                if (ActiveUser.second_dose != null)
                {
                    ActiveUser.second_dose = value;
                    OnPropertyChanged(nameof(SecondDose));
                }
            }
        }

        private ObservableCollection<CovidTest> _testList;
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
            ActiveUser = new User();
            ActiveUser.UserDB = _uDB;
            UpdateUserFullName();

            // Load user tests
            GetUserTestList();
            if(ActiveUser.test_list != null)
            {
                TestList = new ObservableCollection<CovidTest>(ActiveUser.test_list);
            }

            //InitializeAsync(); // Loads userDb into this instance

            // Startup display for user login
            FrameTitle = "My Vaccine Pass";
            InitializeQRAsync();
            NavigateToFrame(new UserMyVaccinePassControlFrame());
            ShowQRFrame();

            PopupContent = new TestResultSuccess();
            // Navigation commands
            LogoutCommand = new RelayCommand(x => NavigateToPage(new LoginPage()));
            NavMyRecordsCommand = new RelayCommand(async x =>
            {
                FrameTitle = "My Records";
                await ActiveUser.GetUserVaccinationsAsync();
                NavigateToFrame(new UserMyRecordsFrame());
            });
            NavMyVaccinePassCommand = new RelayCommand( async x =>
            {
                FrameTitle = "My Vaccine Pass";
                await InitializeQRAsync();
                NavigateToFrame(new UserMyVaccinePassControlFrame());
                ShowQRFrame();
            });

            // Feature Commands
            RequestQRCommand = new RelayCommand(x => RequestQRCode());
            AddTestResultCommand = new RelayCommand(x => AddTestResult());
            ShowPopupCommand = new RelayCommand(x => ShowPopup());
            ClosePopupCommand = new RelayCommand(x => ClosePopup());

            // Test list for TESTING PURPOSES ONLY
            /*
            TestList = new ObservableCollection<CovidTest>();
            generateTest("10-08-2023", false, "RAT");
            generateTest("21-04-2023", true, "PCR"); 
            */
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
        private void generateTest(string date, bool result, string type)
        {
            CovidTest test = new CovidTest();
            test.formatted_test_date = date;
            test.result = result;
            test.test_type = type;

            if (test.result)
            {
                test.formatted_iso_date = ReturnIsoDate(test.formatted_test_date);
            }
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
                    await Task.Delay(100);
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


        private async void AddTestResult()
        {
            try
            {
                CovidTest test1 = new CovidTest();

                int result = ReturnTestResult();
                string testType;

                if(SelectedTestType == "System.Windows.Controls.ComboBoxItem: Rapid Antigen Test (RAT)")
                {
                    testType = "RAT";

                }
                else
                {
                    testType = "PCR";
                }

                test1.formatted_test_date = FormattedSelectedDate;

                if (test1.result)
                {
                    test1.formatted_iso_date = ReturnIsoDate(FormattedSelectedDate);
                }

                await ActiveUser.ReportTestAsync(result, testType);
                ShowSuccessPopup();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
        private int ReturnTestResult()
        {
            if (IsPositiveSelected)
            {
                return 1;
            }
            else if (IsNegativeSelected)
            {
                return 0;
            }

            throw new ArgumentException("Please select result.");
        }

        private string ReturnIsoDate(string formattedDate)
        {
            if (DateTime.TryParseExact(formattedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime originalDate))
            {
                // Add the specified number of days
                DateTime newDate = originalDate.AddDays(10);

                // Convert the new date back to a formatted string
                string newFormattedDate = newDate.ToString("dd-MM-yyyy");

                return newFormattedDate;
            }
            else
            {
                return "Error formatting date.";
            }
        }

        private async Task InitializeQRAsync()
        {
            await ActiveUser.GetQRData();
        }

        private void ShowSuccessPopup()
        {
            //IsPopupOpen = false;
            PopupContent = new TestResultSuccess();
            IsPopupOpen = true;
        }
        private void ShowPopup()
        {
            // Show the popup here
            PopupContent = new AddTestResultPopup();
            IsPositiveSelected = false;
            IsNegativeSelected = false;
            IsPopupOpen = true;
        }
        private void ClosePopup()
        {
            if (IsPopupOpen)
            {
                IsPopupOpen = false;
            }
        }

        private async void GetUserTestList()
        {
            ActiveUser.test_list = await ActiveUser.GetTestsAsync();
        }
    }
}
