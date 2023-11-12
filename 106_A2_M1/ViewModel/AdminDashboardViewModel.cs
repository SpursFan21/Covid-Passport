using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _106_A2_M1.Model;
using _106_A2_M1.Services;

namespace _106_A2_M1.ViewModel
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        private Admin _admin; // Declare an instance of the Admin class MODEL to ViewModel Pipeline
        public List<User> UserList   //MODEL to ViewModel Pipeline
        {
            get { return _admin.user_list; } // Access user_list property from the Admin instance
        }

        public List<Issue> IssueList   //MODEL to ViewModel Pipeline
        {
            get { return _admin.issue_list; } // Access issue_list property from the Admin instance
        }

        public AdminDashboardViewModel()
        {
            _admin = new Admin(); // Initialize the Admin instance MODEL to ViewModel Pipeline
            
            TestAddUsers();
            

        }



        public void TestAddUsers() // TESTING PURPOSES ONLY
        {
            _admin.TestUserGeneration();

        }
    }
}
