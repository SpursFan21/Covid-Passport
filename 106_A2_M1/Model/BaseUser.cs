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
        public int IsolationDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DateOfBirth { get; set; }
        public int NhiNumber { get; set; }


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
        protected void createAccount(string email, string password, string firstName, string lastName, int dob, int nhiNumber)
        {
            // Validate the input data (perform validation based on your specific requirements)
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                // Handle invalid input (throw an exception, return an error code, etc.)
                throw new ArgumentException("Invalid input data. Please provide all required information.");
            }

            // Create a new user account using the provided data
            User newUser = new User
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dob,
                NhiNumber = nhiNumber
            };

            // Perform logic to save the new user account to the database or any other storage mechanism
            // Redirect auto login user and then deliver them to UserDashB
        }
        protected void getIsolationDate(CovidTest covidTest)
        {
            if (covidTest != null)
            {
                // Assuming test_date contains the number of days from today
                // Adding 10 days to test_date
                IsolationDate = covidTest.test_date + 10;
            }
            else
            {
                // Throw a custom exception when covidTest is null
                throw new ArgumentNullException(nameof(covidTest), "CovidTest instance cannot be null.");
            }
        }

        protected void sendPassword(string Password)
        {
            //http receive token ITC
            u_token = "admin";
            //test received token to check if valid 
        }
        public void addTest(int testDate, bool result, string testType)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(testType))
            {
                throw new ArgumentException("Test type cannot be null or empty.", nameof(testType));
            }

            // Generate test_id based on test_date, result, and test_type
            string testId = GenerateTestId(testDate, result, testType);

            // Create a new CovidTest instance with the provided data
            CovidTest newTest = new CovidTest
            {
                test_date = testDate,
                result = result,
                test_type = testType,
                test_id = testId
            };

            // Add the new test instance to test_list
            test_list.Add(newTest);

            // send the new test instance to the backend
            // SendRequestToBackend(newTest);

            // For demonstration purposes
            Console.WriteLine($"Generated test_id: {testId}");
        }

        private string GenerateTestId(int testDate, bool result, string testType)
        {
            // Generate a unique test_id based on test_date, result, and test_type
            string formattedDate = testDate.ToString("yyyyMMdd"); // Format test_date as YYYYMMDD
            string resultIndicator = result ? "1" : "0"; // Use "1" for true and "0" for false
            string testId = $"{formattedDate}_{resultIndicator}_{testType}";

            return testId;
        }
        public void reportIssue(string subject, string description)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Subject and description cannot be null or empty.");
            }

            // Create a new Issue instance with the provided data
            Issue newIssue = new Issue
            {
                issue_id = GenerateIssueId(),
                subject = subject,
                description = description,
                resolve = false, // By default, set resolve to false
                open_date = GetCurrentDate(), // Set open_date to the current date TBC
                closed_date = 0 // By default, set closed_date to 0 (indicating not closed yet)
            };

            // send the new Issue instance to the backend
            // SendRequestToBackend(newIssue);

            // For demonstration purposes
            Console.WriteLine($"Reported issue with issue_id: {newIssue.issue_id}");
        }

        private string GenerateIssueId()
        {
            // Generate a unique issue_id TBC
            return Guid.NewGuid().ToString();
        }

        private int GetCurrentDate()
        {
            // Get the current date as an integer TBC
            return int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        }
        public virtual void UpdateUserDetails(string email, string firstName, string lastName)
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

            // For demonstration purposes
            Console.WriteLine($"Updated basic details for email: {Email}");
        }

        public void logout()
        {
            // Reset the properties to their initial values and perform any necessary cleanup
            u_token = null; // Set the user token to null or an initial value
            image_link = null; // Set the image link to null or an initial value
            IsolationDate = 0; // Reset isolation date to an initial value
            // Reset other properties as needed TBC

            // For demonstration purposes
            Console.WriteLine("User logged out and reset to base state");
        }
    }
}
