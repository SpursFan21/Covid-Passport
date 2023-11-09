using _106_A2_M1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.ViewModel
{
    public class UserDashboardViewModel : ViewModelBase
    {
        private User _user; // Declare an instance of the User class MODEL to ViewModel Pipeline

        public UserDashboardViewModel()
        {
            _user = new User(); // Initialize a new User instance MODEL to ViewModel Pipeline
        }
    }
}
