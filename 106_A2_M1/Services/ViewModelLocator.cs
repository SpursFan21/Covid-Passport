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
        public MainViewModel MainViewModel => new MainViewModel();
        public LoginViewModel LoginViewModel => new LoginViewModel();
        public AdminDashboardViewModel AdminDashboardViewModel => new AdminDashboardViewModel();
        public UserDashboardViewModel UserDashboardViewModel => new UserDashboardViewModel();

        public static ViewModelLocator Instance { get; } = new ViewModelLocator();
    }
}
