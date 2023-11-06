using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    class BaseUser
    {
        protected string id;
        protected int account_type;
        protected string u_token;
        protected string image_link;
        protected Vaccine first_dose;
        protected Vaccine second_dose;
        protected List<CovidTest> test_list;

        protected void login()
        {

        }
        protected void createAccount() 
        {
        
        }
        protected void getIsolationDate()
        {

        }
        protected void sendPassword()
        {

        }
        protected void addTest()
        {

        }
        protected void reportIssue()
        {

        }
        protected virtual void updateUserDetails()
        {

        }
        protected void logout()
        {

        }
    }
}
