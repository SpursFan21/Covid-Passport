using _106_A2_M1.Model;
using _106_A2_M1.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel 
        {
            get { return _currentViewModel; }
            set
            {
                if(_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }
        private BaseUser _user; // Declare _user at the class level MODEL to ViewModel PipeLine
        public MainViewModel()
        {
            CurrentViewModel = ViewModelLocator.Instance.LoginViewModel;
            _user = new BaseUser(); // Initialize a new BaseUser instance MODEL to ViewModel Pipeline
            _user.test_list = new List<CovidTest>(); // Initialize the test_list property MODEL to ViewModel Pipeline
        }
       
        public void NavigateToAdminDashboard()
        {
            CurrentViewModel = ViewModelLocator.Instance.AdminDashboardViewModel;
        }

        public void NavigateToUserDashboard()
        {
            CurrentViewModel = ViewModelLocator.Instance.UserDashboardViewModel;
        }
        public BaseUser User { get; set; }

        //MODEL to ViewModel Pipeline
        public UserDB DbMember
        {
            get { return _user.db_member; }
            set { _user.db_member = value; }
        }

        //MODEL to ViewModel Pipeline
        public Vaccine FirstDose
        {
            get { return _user.first_dose; }
            set { _user.first_dose = value; }
        }

        //MODEL to ViewModel Pipeline
        public Vaccine SecondDose
        {
            get { return _user.second_dose; }
            set { _user.second_dose = value; }
        }
    }
}
