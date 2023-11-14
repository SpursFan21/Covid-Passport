using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using _106_A2_M1.Model;
using _106_A2_M1.Services;
using _106_A2_M1.View;
using _106_A2_M1.View.AdminFrames;
using _106_A2_M1.View.Pages;

namespace _106_A2_M1.ViewModel
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        private ICommand _userSearchCommand;
        public ICommand LogoutCommand { get; }
        public ICommand QRQueueCommand { get; }
        public ICommand UserDirectoryCommand { get; }
        public ICommand IssueQueueCommand { get; }

        public ICommand MarkAsResolvedCommand { get; }

        private Admin _admin; // Declare an instance of the Admin class MODEL to ViewModel Pipeline
        private ObservableCollection<User> _userList; // Change the type to ObservableCollection<User>
        private ObservableCollection<Issue> _issueList;
        private UserControl _currentDisplayFrame;
        private string _frameTitle;

        private Issue _testIssue = new Issue() { subject = "woohoo", description = "not again", resolve = false };
        public ICommand UserSearchCommand
        {
            get
            {
                if (_userSearchCommand == null)
                    _userSearchCommand = new RelayCommand(param => UserDirSearch(), null);

                return _userSearchCommand;
            }
        }
        public ObservableCollection<User> UserList   //MODEL to ViewModel Pipeline
        {
            get { return _userList; } // Access the ObservableCollection
            set
            {
                _userList = value;
                OnPropertyChanged(nameof(UserList)); // Notify property changed to update the UI
            }
        }
        public ObservableCollection<Issue> IssueList   //MODEL to ViewModel Pipeline
        {
            get => _issueList;  // Access the ObservableCollection
            set
            {
                _issueList = value;
                OnPropertyChanged(nameof(IssueList)); // Notify property changed to update the UI
            }
        }
        public string FrameTitle
        {
            get { return _frameTitle; } // Access the string
            set
            {
                _frameTitle = value;
                OnPropertyChanged(nameof(FrameTitle)); // Notify property changed to update the UI
            }
        }
        public Issue TestIssue
        {
            get => _testIssue;
            set
            {
                _testIssue = value;
                OnPropertyChanged(nameof(TestIssue));
            }
        }

        public AdminDashboardViewModel()
        {
            _admin = new Admin(); // Initialize the Admin instance MODEL to ViewModel Pipeline
            UserList = new ObservableCollection<User>(_admin.user_list); // Initialize the UserList
            IssueList = new ObservableCollection<Issue>(_admin.issue_list); // Initialize IssueList

            TestAddUsers();
            TestAddIssues();
            TestIssue = new Issue() { subject = "this is my problem", description = "please help me with this problem please help me with this problem please help me with this problem please help me with this problem please help me with this problem" }; ;
            // Set the default frame to UserDirectoryFrame
            FrameTitle = "User Directory";
            CurrentDisplayFrame = new AdminUserDirectoryFrame();
            CurrentDisplayFrame.DataContext = this;

            LogoutCommand = new RelayCommand(x => NavigateToPage(new LoginPage()));
            QRQueueCommand = new RelayCommand(x => 
                { 
                    FrameTitle = "QR Code Approval Queue";
                    NavigateToFrame(new AdminQRQueueFrame()); 
                });
            UserDirectoryCommand = new RelayCommand(x =>
            {
                FrameTitle = "User Directory";
                NavigateToFrame(new AdminUserDirectoryFrame());
            });
            IssueQueueCommand = new RelayCommand(x =>
            {
                FrameTitle = "Issue Report Management";
                NavigateToFrame(new AdminIssueManagementFrame());
            });
            MarkAsResolvedCommand = new RelayCommand(x => MarkAsResolved(x));
        }

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

        public void TestAddUsers() // TESTING PURPOSES ONLY
        {
            _admin.TestUserGeneration();
            UserList = new ObservableCollection<User>(_admin.user_list); // Update the ObservableCollection after adding users
        }
        public void TestAddIssues() // TESTING PURPOSES ONLY
        {
            _admin.TestIssueGeneration();
            IssueList = new ObservableCollection<Issue>(_admin.issue_list); // Update the ObservableCollection after adding users
        }

        public void UserDirSearch()
        {

        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageBox.Show("Data Context Working!");
            }
        }

        public void MarkAsResolved(object parameter)
        {
            if (parameter is Issue issue)
            {
                issue.resolve = true;

                OnPropertyChanged(nameof(IssueList));
            }
        }
    }

}
