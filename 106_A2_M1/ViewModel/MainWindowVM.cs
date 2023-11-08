using _106_A2_M1.View;
using _106_A2_M1.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _106_A2_M1.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        public MainWindowVM()
        {
            //Very first default page when the app load
            CurrentDisplayPage = new LoginPage();
        }

        private Page _currentDisplayPage;
        public Page CurrentDisplayPage
        {
            get
            {
                return _currentDisplayPage;
            }

            set
            {
                _currentDisplayPage = value;
                OnPropertyChanged("CurrentDisplayPage");
            }
        }
    }
}
