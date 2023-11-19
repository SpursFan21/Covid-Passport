using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    //data members coming from database
    public class UserDB
    {
        public string id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string dob { get; set; }
        public int nhi_num { get; set; }
        public int qr_status { get; set; }
        public int issue_ct { get; set; }
        public int test_ct { get; set; }
        public int vaccine_status { get; set; }

        public Dictionary<string, object> GetUserData()
        {
            return new Dictionary<string, object>
        {
            { nameof(id), id },
            { nameof(email), email },
            { nameof(first_name), first_name },
            { nameof(last_name), last_name },
            { nameof(dob), dob },
            { nameof(nhi_num), nhi_num },
            { nameof(qr_status), qr_status },
            { nameof(issue_ct), issue_ct },
            { nameof(test_ct), test_ct },
            { nameof(vaccine_status), vaccine_status }
        };
        }
    }
   
}
