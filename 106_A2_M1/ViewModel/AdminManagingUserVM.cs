using _106_A2_M1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.ViewModel
{
    public class AdminManagingUserVM : AdminDashboardViewModel
    {
        
        private Admin _admin;
        private User _activeUser;
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


    }
}
