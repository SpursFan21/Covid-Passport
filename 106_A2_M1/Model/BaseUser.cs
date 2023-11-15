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


        protected async Task Login(string email, string password, string u_token)
        {
            // Hardcoded user tokens for testing purposes
            if (u_token == "admin") // Login as Admin
            {
                // Call the LoginAsync method from SingletonClient to perform authentication
                int loginResult = await SingletonClient.Instance.LoginAsync(email, password, "adminAuthToken");

                if (loginResult == 1)
                {
                    // Admin authentication successful
                    Admin admin = new Admin();
                    admin.u_token = "adminAuthToken"; // Use the hardcoded admin token or set it based on the authentication result

                    // Need to receive database content for admin ITC
                    // Send Admin to Admin dashboard Via ModelView NAV
                }
                else
                {
                    // Admin authentication failed
                    Console.WriteLine("Admin authentication failed!");
                }
            }
            else // Login as User
            {
                // Call the LoginAsync method from SingletonClient to perform authentication
                int loginResult = await SingletonClient.Instance.LoginAsync(email, password, "userAuthToken");

                if (loginResult == 2)
                {
                    // User authentication successful
                    User user = new User();
                    user.u_token = "userAuthToken"; // Use the hardcoded user token or set it based on the authentication result

                    // Send User to User Dashboard Via ModelView NAV
                }
                else
                {
                    // User authentication failed
                    Console.WriteLine("User authentication failed!");
                }
            }
        }
        protected async Task<int> CreateAccountAsync(string email, string password, string firstName, string lastName, int dob, int nhiNumber)
        {
            try
            {
                // Use SingletonClient to create a new account
                SingletonClient singletonClient = SingletonClient.Instance;
                int result = await singletonClient.CreateAccountAsync(email, password, firstName, lastName, dob, nhiNumber);

                // Redirect auto-login logic and navigate to UserDashB based on the result
                if (result == 1)
                {
                    // Account creation successful, perform auto-login logic here
                    await Login(email, password, "userAuthToken");
                    // Redirect to UserDashB Via ModelView NAV
                }
                else
                {
                    Console.WriteLine("Account creation failed!");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0; // Indicate failure due to an exception
            }
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

            // send the new test instance to the backend TBC
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
        public async Task ReportIssueAsync(string subject, string description)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(description))
                {
                    throw new ArgumentException("Subject and description cannot be null or empty.");
                }

                // Create a new Issue instance with the provided data
                Issue newIssue = new Issue
                {
                    subject = subject,
                    description = description,
                };

                // Use the SingletonClient to report the issue
                await SingletonClient.Instance.ReportIssueAsync(newIssue);

                // For demonstration purposes
                Console.WriteLine($"Reported issue with subject: {subject}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reporting the issue: {ex.Message}");
            }
        }

        // Overloaded method with additional parameters
        public virtual void UpdateUserDetails(string email, string firstName, string lastName, int dateOfBirth, int nhiNumber)
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
            Console.WriteLine($"Updated details for email: {Email}, Date of Birth: {dateOfBirth}, NHI Number: {nhiNumber}");
        }
        public void logout()
        {
            // Reset the properties to their initial values and perform any necessary cleanup
            u_token = null; // Set the user token to null or an initial value
            image_link = null; // Set the image link to null or an initial value
            IsolationDate = 0; // Reset isolation date to an initial value
            SingletonClient.Instance.Dispose();

            // For demonstration purposes
            Console.WriteLine("User logged out and reset to base state");
        }
        public void deleteVaccination(string doseId, string doseType)
        {
            // Determine which dose to delete based on the specified doseType
            Vaccine vaccineToDelete = doseType.ToLower() == "first"
                ? first_dose
                : (doseType.ToLower() == "second" ? second_dose : null);

            if (vaccineToDelete != null && vaccineToDelete.dose_id == doseId)
            {
                // Remove the specified dose
                if (doseType.ToLower() == "first")
                {
                    first_dose = null;
                }
                else if (doseType.ToLower() == "second")
                {
                    second_dose = null;
                }

                // Additional logic to notify backend or perform any other necessary actions TBC
                // SendRequestToDeleteVaccination(vaccineToDelete); TBC 

                Console.WriteLine($"Vaccination ({doseType} dose) with dose ID {doseId} deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Vaccination ({doseType} dose) with dose ID {doseId} not found.");
            }
        }
    }
}
