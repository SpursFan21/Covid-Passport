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
        public MainViewModel()
        {
            CurrentViewModel = ViewModelLocator.Instance.LoginViewModel;
        }

        public void NavigateToAdminDashboard()
        {
            CurrentViewModel = ViewModelLocator.Instance.AdminDashboardViewModel;
        }

        public void NavigateToUserDashboard()
        {
            CurrentViewModel = ViewModelLocator.Instance.UserDashboardViewModel;
        }

    }
}
