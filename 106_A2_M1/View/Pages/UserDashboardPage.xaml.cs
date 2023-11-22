using _106_A2_M1.Model;
using _106_A2_M1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _106_A2_M1.View.Pages
{
    /// <summary>
    /// Interaction logic for UserDashboardPage.xaml
    /// </summary>
    public partial class UserDashboardPage : Page
    {
        private UserDB uDB;
        public UserDashboardPage(UserDB _uDB)
        {
            uDB = new UserDB();
            uDB = _uDB;
            InitializeComponent();
            DataContext = new UserDashboardViewModel(uDB);
        }
        private void ClosePopupCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Execute the ClosePopupCommand in the ViewModel
            if (DataContext is UserDashboardViewModel viewModel)
            {
                viewModel.ClosePopupCommand.Execute(null);
            }
        }

    }
}
