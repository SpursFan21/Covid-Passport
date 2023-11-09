using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class User : BaseUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DateOfBirth { get; set; }
        public int NhiNumber { get; set; }
        public void requestQR()
        {

        }
        protected override void updateUserDetails()
        {

        }

        internal static void SetPassword(string value)
        {
            throw new NotImplementedException();
        }
    }
}
