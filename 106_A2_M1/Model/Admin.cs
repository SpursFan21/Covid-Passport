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

        public Admin()
        {
            user_list = new List<User>();
            issue_list = new List<Issue>();
        }

        public void TestUserGeneration() // TESTING PURPOSES ONLY
        {
            User user1 = new User() { FirstName = "Hank", DateOfBirth = 01011972, Email = "hank@gmail.com" };
            User user2 = new User() { FirstName = "Marie", DateOfBirth = 02021978, Email = "marie@gmail.com" };
            user_list.Add(user1);
            user_list.Add(user2);

        }

        public void TestIssueGeneration() // TESTINF PURPOSES ONLY
        {
            Issue issue1 = new Issue()
            {
                subject = "I have a problem",
                description = "Where do I log out of my account, I have been logged in for 72 days.",
                resolve = false
            };
            Issue issue2 = new Issue()
            {
                subject = "I too, have a problem",
                description = "How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!How can I update my details? The button does not work!",
                resolve = false
            };
            issue_list.Add(issue1);
            issue_list.Add(issue2);
        }
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
