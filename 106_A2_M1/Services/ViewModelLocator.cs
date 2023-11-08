using _106_A2_M1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Services
{
    public class ViewModelLocator
    {
        private static ViewModelLocator _instance;

        public static ViewModelLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ViewModelLocator();
                }
                return _instance;
            }
        }

        // ViewModels
        public LoginViewModel LoginViewModel => new LoginViewModel();
        public AdminDashboardViewModel AdminDashboardViewModel => new AdminDashboardViewModel();
        public UserDashboardViewModel UserDashboardViewModel => new UserDashboardViewModel();
    }
}
