using _106_A2_M1.ViewModel;
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
        public int account_type { get; set; }
        public string u_token { get; set; }
        public string image_link { get; set; }
        public int IsolationDate { get; set; }
        public static string password { get; set; }

        // Private field for UserType
        private int _userType = 0;

        // Property for UserType
        public int UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        public UserDB _userDB;

        // Expose UserDB through a property
        public UserDB UserDB
        {
            get { return _userDB; }
            set { _userDB = value; }
        }

        //data objects
        public Vaccine first_dose { get; set; }
        public Vaccine second_dose { get; set; }
        public List<CovidTest> test_list { get; set; }
      

        public async Task GetLoginAsync(string email, string password)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Both email and password are required for login.");
                    return;
                }

                // Call the Login method with the provided email and password
                await Login(email, password);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                // Log the exception or handle it based on your application's needs
            }
        }


        public async Task Login(string email, string password)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Email, password");
                    return;
                }

                // Call the LoginAsync method from SingletonClient to perform authentication
                int loginResult = await SingletonClient.Instance.LoginAsync(email, password);

                if (loginResult == 1)
                {
                    // Admin authentication successful
                    Admin admin = new Admin();
                    admin.u_token = u_token;
                    UserType = 1; // used for ModelView Nav Command 

                    // Send Admin to Admin dashboard Via ModelView NAV
                }
                else if (loginResult == 2)
                {
                    // User authentication successful
                    User user = new User();
                    user.u_token = u_token;
                    UserType = 2; // used for ModelView NAV command

                    // Send User to User Dashboard Via ModelView NAV
                }
                else
                {
                    // Authentication failed
                    Console.WriteLine("Authentication failed!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                // Log the exception or handle it based on your application's needs
            }
        }

        public static async Task<UserDB> RetrieveUserInformationAsync()
        {
            try
            {
                // Call the private method in SingletonClient to get user information
                UserDB userDB = await SingletonClient.Instance.GetUserInformationAsync();

                // You can perform additional actions here if needed

                return userDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving user information: {ex.Message}");
                return null;
            }
        }

        public async Task CreateAccountAsync(string email, string password, string firstName, string lastName, int dob, string nhiNum)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                {
                    throw new ArgumentException("Email, first name, and last name cannot be null or empty.");
                }

                // Call the CreateAccountAsync method in SingletonClient
                UserDB user = await SingletonClient.Instance.CreateAccountAsync(email, password, firstName, lastName, dob, nhiNum);

                // Set the unique data member using the class name
                BaseUser.password = password;

                // Create an instance of UserDB
                UserDB userDB = new UserDB();

                // Store the user information in UserDB
                userDB.email = email;
                userDB.first_name = firstName;
                userDB.last_name = lastName;
                userDB.dob = dob; // Assuming dob is already an int
                userDB.nhi_num = nhiNum;

                // Perform any additional logic for account creation
                Console.WriteLine("Account created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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

        // Overloaded method with additional parameters
        public virtual void UpdateUserDetails(string email, string firstName, string lastName, string dateOfBirth, int nhiNumber)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Email, first name, and last name cannot be null or empty.");
            }

            // Update the basic details
            UserDB.email = email;
            UserDB.first_name = firstName;
            UserDB.last_name = lastName;

            // For demonstration purposes
            Console.WriteLine($"Updated details for email: {email}, Date of Birth: {dateOfBirth}, NHI Number: {nhiNumber}");
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
