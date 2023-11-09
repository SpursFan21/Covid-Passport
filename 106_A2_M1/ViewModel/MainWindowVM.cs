using _106_A2_M1.Model;
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
        private BaseUser _user; // Declare _user at the class level MODEL to ViewModel PipeLine
        public MainWindowVM()
        {
            //Very first default page when the app load
            CurrentDisplayPage = new LoginPage();
            _user = new BaseUser(); // Initialize a new BaseUser instance MODEL to ViewModel Pipeline
            _user.test_list = new List<CovidTest>(); // Initialize the test_list property MODEL to ViewModel Pipeline
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

        public BaseUser User { get; set; } // MODEL to ViewModel Pipeline

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

