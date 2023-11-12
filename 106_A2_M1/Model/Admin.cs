using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class Admin : BaseUser
    {
        public string user_id { get; set; }

        public List<User> user_list { get; set; }
        public List<Issue> issue_list { get; set; }

        public override void UpdateUserDetails(string email, string firstName, string lastName, int dateOfBirth, int nhiNumber)//TBC
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Email, first name, and last name cannot be null or empty.");
            }

            // Update the basic details
            Email = email;
            FirstName = firstName;
            LastName = lastName;

            // Additional logic for updating Admin-specific details
            DateOfBirth = dateOfBirth;
            NhiNumber = nhiNumber;

            // For demonstration purposes
            Console.WriteLine($"Updated details for email: {Email}, Date of Birth: {DateOfBirth}, NHI Number: {NhiNumber}");
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
