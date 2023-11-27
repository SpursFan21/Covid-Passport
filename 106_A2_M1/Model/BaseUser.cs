using _106_A2_M1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        // Property to store the QR code image link
        public string qrCodeU { get; private set; }
       
        //URL Storage
        public string storedQRCodeImageURL;

        //URl expiry Date storage packed
        public ulong storedExp;

        //URL Expiry date storage un-packed
        public string UrlExpDate;

        //Image Storage
        public byte[] storedImageData;


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
      
        public BaseUser()
        {
            first_dose = new Vaccine();
            second_dose = new Vaccine();
        }

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
        /*


        public async Task<UserDB> RetrieveUserInformationAsync()

        {
            try
            {
                // Call the method in SingletonClient to get user information
                UserDB userDB = await SingletonClient.Instance.GetUserInformationAsync();

                // You can perform additional actions here if needed

                return userDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving user information: {ex.Message}");
                return null;
            }
        }*/

        public async Task<UserDB> RetrieveUserInformationAsync()
        {
            try
            {
                // Call the method in SingletonClient to get user information
                UserDB userDB = await SingletonClient.Instance.GetUserInformationAsync();

                // Check if userDB is not null and dob is a valid Unix timestamp
                if (userDB != null && userDB.dob > 0)
                {
                    // Convert Unix timestamp to formatted date string
                    userDB.FormattedDOB = FormatUnixTimestampDob(userDB.dob);
                }

                return userDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving user information: {ex.Message}");
                return null;
            }
        }

        //Timestamp to string converter for users date of birth
        public static string FormatUnixTimestampDob(long unixTimestamp)
        {
            try
            {
                // Unix Epoch time starts from January 1, 1970
                DateTimeOffset epochTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

                // Add the Unix timestamp in milliseconds to get the actual DateTimeOffset
                DateTimeOffset dateTimeOffset = epochTime.AddMilliseconds(unixTimestamp);

                // Format the DateTimeOffset as a string
                string formattedDateTimeDob = dateTimeOffset.ToString("dd-MM-yyyy");

                return formattedDateTimeDob;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while formatting Unix timestamp: {ex.Message}");
                return string.Empty;
            }
        }

        public async Task CreateAccountAsync(string email, string password, string firstName, string lastName, DateTime dobDT, string nhiNum)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                {
                    throw new ArgumentException("Email, first name, and last name cannot be null or empty.");
                }

                // Change DateTime to Unix timestamp (number of seconds since Unix epoch)
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                TimeSpan elapsedTime = dobDT.ToUniversalTime() - epoch;
                int dob = (int)elapsedTime.TotalSeconds * 1000;

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
                userDB.dob = dob; // Unix timestamp
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
                IsolationDate = (int)covidTest.test_date + 10;
            }
            else
            {
                // Throw a custom exception when covidTest is null
                throw new ArgumentNullException(nameof(covidTest), "CovidTest instance cannot be null.");
            }
        }

        public async Task AddTestAsync(long testDate, bool result, string testType)
        {
            try
            {
                // Use SingletonClient to make a POST request to add a new test
                bool isSuccess = await SingletonClient.Instance.AddTestAsync(testDate, result, testType);

                if (isSuccess)
                {
                    Console.WriteLine("Test added successfully.");

                    // Create a new CovidTest instance with the provided data
                    CovidTest newTest = new CovidTest
                    {
                        test_date = testDate,
                        result = result,
                        test_type = testType,
                    };

                    // Add the new test instance to test_list
                    test_list.Add(newTest);
                }
                else
                {
                    Console.WriteLine("Failed to add test.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private string GenerateTestId(long testDate, bool result, string testType)
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

        /* public async Task<(string, ulong)> RetrieveQRCodeImageURLAsync()
         {
             try
             {
                 // Use SingletonClient to retrieve the QR code image URL asynchronously
                 var (qrCodeURL, qrCodeExe) = await SingletonClient.Instance.RetrieveQRCodeImageURLAsync();

                 if (qrCodeURL != null)
                 {
                     Console.WriteLine($"QR Code Image URL: {qrCodeURL}");
                     Console.WriteLine($"QR Code Expiration: {qrCodeExe}");

                     // Store the QR code image URL in the field
                     storedQRCodeImageURL = qrCodeURL;
                     storedExp = qrCodeExe;

                     return (qrCodeURL, qrCodeExe);
                 }
                 else
                 {
                     Console.WriteLine("QR Code Image URL not found.");
                     return (null, 0);
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"An error occurred: {ex.Message}");
                 return (null, 0);
             }
         }*/

        public async Task<(string, string)> RetrieveQRCodeImageURLAsync()
        {
            try
            {
                // Use SingletonClient to retrieve the QR code image URL asynchronously
                var (qrCodeURL, qrCodeExe) = await SingletonClient.Instance.RetrieveQRCodeImageURLAsync();

                if (qrCodeURL != null)
                {
                    Console.WriteLine($"QR Code Image URL: {qrCodeURL}");

                    // Format the expiration timestamp as a string
                    string formattedExp = FormatUnixTimestamp((long)qrCodeExe); // Corrected this line
                    Console.WriteLine($"QR Code Expiration: {formattedExp}");

                    // Store the QR code image URL and formatted expiration in the fields
                    storedQRCodeImageURL = qrCodeURL;
                    storedExp = qrCodeExe;

                    return (qrCodeURL, formattedExp);
                }
                else
                {
                    Console.WriteLine("QR Code Image URL not found.");
                    return (null, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return (null, null);
            }
        }

        public string FormatUnixTimestamp(long unixTimestamp)
        {
            try
            {
                // Unix Epoch time starts from January 1, 1970
                DateTimeOffset epochTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

                // Add the Unix timestamp in milliseconds to get the actual DateTimeOffset
                DateTimeOffset dateTimeOffset = epochTime.AddMilliseconds(unixTimestamp);

                // Format the DateTimeOffset as a string
                string formattedDateTime = dateTimeOffset.ToString("dd-MM-yyyy");

                UrlExpDate = formattedDateTime;
                return formattedDateTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while formatting Unix timestamp: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetFormattedExpiration()
        {
            return FormatUnixTimestamp((long)storedExp);
        }

        public async Task RetrieveQRCodeImageAsync()
        {
            try
            {
                // Use SingletonClient to retrieve the QR code image asynchronously
                byte[] imageData = await SingletonClient.Instance.RetrieveQRCodeImageAsync();

                if (imageData != null && imageData.Length > 0)
                {
                    //method to save the image
                    storedImageData = imageData;

                    Console.WriteLine("QR Code Image retrieved successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to retrieve QR Code Image.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task GetQRData()
        {
            try
            {
                //await RetrieveQRCodeImageAsync();
                await RetrieveQRCodeImageURLAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception was caught!: {ex.Message}");
            }
        }

        //retrieve the storedImageData
        public byte[] GetStoredImageData()
        {
            return storedImageData;
        }

        //retrieve the storedQRCodeImageURL
        public string GetStoredQRCodeImageURL()
        {
            return storedQRCodeImageURL;
        }

       /* public async Task<List<CovidTest>> GetTestsAsync()
        {
            try
            {
                // Use SingletonClient to get test information through a GET request
                List<CovidTest> testInfoList = await SingletonClient.Instance.GetTestsAsync();

                if (testInfoList != null && testInfoList.Count > 0)
                {
                    foreach (var testInfo in testInfoList)
                    {
                        Console.WriteLine("Test Information:");
                        Console.WriteLine($"Test ID: {testInfo.test_id}");
                        Console.WriteLine($"Test Date: {testInfo.test_date}");
                        Console.WriteLine($"Result: {testInfo.result}");
                        Console.WriteLine($"Test Type: {testInfo.test_type}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve test information from the backend.");
                }

                test_list = testInfoList;
                return testInfoList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }*/

        public async Task<List<CovidTest>> GetTestsAsync()//attemp 2
        {
            try
            {
                // Use SingletonClient to get test information through a GET request
                List<CovidTest> testInfoList = await SingletonClient.Instance.GetTestsAsync();

                if (testInfoList != null && testInfoList.Count > 0)
                {
                    foreach (var testInfo in testInfoList)
                    {
                        Console.WriteLine("Test Information:");
                        Console.WriteLine($"Result: {testInfo.result}");
                        Console.WriteLine($"Test Type: {testInfo.test_type}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve test information from the backend.");
                }

                test_list = testInfoList;
                return testInfoList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task GetUserVaccinationsAsync()
        {
            try
            {
                // Use SingletonClient to get vaccination information through a GET request
                List<Vaccine> vaccinationData = await SingletonClient.Instance.GetUserVaccinationsAsync();

                if (vaccinationData != null && vaccinationData.Count > 0)
                {
                    // Check if the user has received the first dose
                    if (vaccinationData.Count >= 1)
                    {
                        Console.WriteLine("First Dose Information:");
                        Console.WriteLine($"Dose ID: {vaccinationData[0].dose_id}");
                        Console.WriteLine($"Date Administered: {vaccinationData[0].date_administered}");
                        Console.WriteLine($"Brand: {vaccinationData[0].brand}");
                        Console.WriteLine($"Location: {vaccinationData[0].location}");

                        // Set data to this object
                        first_dose.dose_id = vaccinationData[0].dose_id;
                        first_dose.date_administered = vaccinationData[0].date_administered;
                        first_dose.brand = vaccinationData[0].brand;
                        first_dose.location = vaccinationData[0].location;
                    }
                    else
                    {
                        Console.WriteLine("No information available for the first dose.");
                    }

                    // Check if the user has received the second dose
                    if (vaccinationData.Count >= 2)
                    {
                        Console.WriteLine("\nSecond Dose Information:");
                        Console.WriteLine($"Dose ID: {vaccinationData[1].dose_id}");
                        Console.WriteLine($"Date Administered: {vaccinationData[1].date_administered}");
                        Console.WriteLine($"Brand: {vaccinationData[1].brand}");
                        Console.WriteLine($"Location: {vaccinationData[1].location}");

                        // Set data to this object
                        second_dose.dose_id = vaccinationData[1].dose_id;
                        second_dose.date_administered = vaccinationData[1].date_administered;
                        second_dose.brand = vaccinationData[1].brand;
                        second_dose.location = vaccinationData[1].location;
                    }
                    else
                    {
                        Console.WriteLine("\nNo information available for the second dose.");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve vaccination information from the backend.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        /*
                public async Task ReportTestAsync(DateTime testDate, bool result, string testType)
                {
                    try
                    {
                        // Validate input parameters
                        if (string.IsNullOrEmpty(testType))
                        {
                            throw new ArgumentException("Invalid test details. Please provide valid information.");
                        }

                        // Convert DateTime to Unix timestamp
                        long unixTimestamp = testDate.ToUnixTimestamp();

                        // Create a new CovidTest object
                        CovidTest testReport = new CovidTest
                        {
                            test_date = unixTimestamp,
                            result = result,
                            test_type = testType
                        };

                        // Use SingletonClient to report the test details through a POST request
                        bool isReported = await SingletonClient.Instance.ReportTestAsync(testReport);

                        if (isReported)
                        {
                            Console.WriteLine("Test reported successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to report the test to the backend.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }*/
        public async Task ReportTestAsync(int result, string testType)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(testType))
                {
                    throw new ArgumentException("Invalid test details. Please provide valid information.");
                }

                // Use SingletonClient to report the test details through a POST request
                bool isReported = await SingletonClient.Instance.ReportTestAsync(result, testType);

                if (isReported)
                {
                    Console.WriteLine("Test reported successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to report the test to the backend.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
