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


        private User _user; // Declare an instance of the User class MODEL to ViewModel Pipeline
        private UserDB _userDB;

        public string FirstName
        {
            get => _user.UserDB.first_name;
            set
            {
                if (_user.UserDB.first_name != value)
                {
                    _user.UserDB.first_name = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => _user.UserDB.last_name;
            set
            {
                if (_user.UserDB.last_name != value)
                {
                    _user.UserDB.last_name = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string UserDOB
        {
            get => _user.UserDB.dob;
            set
            {
                if (_user.UserDB.dob != value)
                {
                    _user.UserDB.dob = value;
                    OnPropertyChanged(nameof(UserDOB));
                }
            }
        }

        public string UserEmail
        {
            get => _user.UserDB.email;
            set
            {
                if (_user.UserDB.email != value)
                {
                    _user.UserDB.email = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }

        public int UserNHI
        {
            get => _user.UserDB.nhi_num;
            set
            {
                if (_user.UserDB.nhi_num != value)
                {
                    _user.UserDB.nhi_num = value;
                    OnPropertyChanged(nameof(UserNHI));
                }
            }
        }

        public string UserFullName { get; private set; }



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
            // Access UserDB data from User in MODEL    
            //UserDB userDbData = _user.UserInformation;
            //UserFullName = userDBData.first_name +" " + userDBData.last_name;
            _userDB = new UserDB();
            generateUserData("Hank", "Schrader", "31 Aug 1970", "hank@DIA.com", 1234567);
            UpdateUserFullName();


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

        private void generateUserData(string fname, string lname, string dob, string email, int nhi)
        {
            _userDB.first_name = fname;
            _userDB.last_name = lname;
            _userDB.dob = dob;
            _userDB.email = email;
            _userDB.nhi_num = nhi;
            _user.UserDB = _userDB;
        }

        private void UpdateUserFullName()
        {
            UserFullName = $"{FirstName} {LastName}";
            OnPropertyChanged(nameof(UserFullName));
        }
    }
}
