using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    class Admin : BaseUser
    {
        public string user_id { get; set; }
        public List<User> user_list { get; set; }
        public List<Issue> issue_list { get; set; }

        protected override void updateUserDetails()
        {

        }
        public void manageUser()
        {

        }
        public void deleteVaccination()
        {

        }
        public void addVaccination()
        {

        }
        public void deleteTest()
        { 
        
        }
        public void generateQR()
        {

        }
        public void deleteQR()
        {

        }
        public void denyQR()
        {

        }
        public void searchDirectory()
        {

        }
        public void searchQR()
        {

        }
        public void searchIssue()
        {

        }
        public void updateIssue()
        {

        }

    }
}
