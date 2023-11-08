using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class BaseUser
    {

        //unique data members
        public string id { get; set; }
        public int account_type { get; set; }
        public string u_token { get; set; }
        public string image_link { get; set; }

        //data objects
        public Vaccine first_dose { get; set; }
        public Vaccine second_dose { get; set; }
        public List<CovidTest> test_list { get; set; }
        public UserDB db_member { get; set; }

        protected void login(string Password)
        {
            sendPassword(Password); //ITC
            //Hardcoded user tokens for testing purposes
            if (u_token == "admin") // Login as Admin
            {
                Admin admin = new Admin();
                admin.u_token = this.u_token; // BaseUser class token updated to admin token
                //need to receive database content for admin use ITC
                //send Admin to Admin dashboard
            }
            // Componet to receive invalid PW from backend ITC
            else// Login as User
            {
                User user = new User();
                user.u_token = this.u_token;
                //need to receive database content for user ITC
                //send User to User Dashboard
            }
        }
        protected void createAccount() 
        {
        
        }
        protected void getIsolationDate()
        {

        }
        protected void sendPassword(string Password)
        {
            //http receive token ITC
            u_token = "admin";
            //test received token to check if valid 
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
