using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using _106_A2_M1.Interfaces.Auth;
using Newtonsoft.Json;

namespace _106_A2_M1.Model
{
    public class SingletonClient : IDisposable
    {
        // A static object used for locking to ensure thread safety during instance creation
        private static readonly object lockObject = new object();

        // The single instance of the SingletonClient
        private static SingletonClient instance;

        // The HttpClient instance used for making HTTP requests
        private HttpClient _client;

        // Private constructor to prevent external instantiation
        private SingletonClient()
        {
            // Initialize HttpClient if needed
            this._client = new HttpClient();
        }
        // Public property to get the instance of SingletonClient
        public static SingletonClient Instance
        {
            get
            {
                // Using a lock to ensure that only one thread can enter this block at a time
                lock (lockObject)
                {
                    // Check if the instance is null (not created yet)
                    if (instance == null)
                        // If it's null, create a new instance
                        instance = new SingletonClient();
                    // Return the instance (either newly created or existing)
                    return instance;
                }
            }
        }

         // Property to store UserDB instance
    public UserDB UserInformation { get; private set; }


        public async Task<int> LoginAsync(string email, string password)
        {
            // Create a new HttpClient instance if it doesn't exist
            if (this._client == null)
            {
                this._client = new HttpClient();
            }

            string apiUrl = "https://cse106-backend.d3rpp.dev/auth/login";

            try
            {
                // Create a string containing the login data
                LoginRequest loginData = new LoginRequest()
                {
                    email = email,
                    password = password
                };

                // Convert the data to StringContent
                var stringContent = new StringContent(loginData.ToJSONString(), Encoding.UTF8, "application/json");
                
                // Add the Authorization header with the provided authToken
                // this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                // Make a POST request with the login data and Authorization header
                HttpResponseMessage response = await this._client.PostAsync(apiUrl, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);

                    AdminAuth admin_auth = AdminAuth.FromJSONString(content);
                  
                    byte[] bytes = Convert.FromBase64String(admin_auth.token.Split('.')[1]);
                    string decoded_string = Encoding.UTF8.GetString(bytes);

                    Console.WriteLine(decoded_string);

                    this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", admin_auth.token);

                    if (decoded_string.Contains("admin"))
                    {
                        // is admin
                        return 1;
                    } 
                    else
                    {
                        // Retrieve user information
                        UserDB userDB = await GetUserInformationAsync(); // Implement this method to fetch user information
                        this.UserInformation = userDB;
                        return 2;
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }
        public async Task<UserDB> GetUserInformationAsync()
        {
            try
            {
                // Make a request to your backend API to retrieve user information
                HttpResponseMessage userResponse = await this._client.GetAsync("https://cse106-backend.d3rpp.dev/api/user");

                if (userResponse.IsSuccessStatusCode)
                {
                    // Deserialize the response content to obtain the user information
                    string content = await userResponse.Content.ReadAsStringAsync();
                    var userData = JsonConvert.DeserializeObject<UserDataFromBackend>(content);

                    // Map the backend data to UserDB class
                    UserDB userDB = new UserDB
                    {
                        email = userData.email,
                        first_name = userData.given_name,
                        last_name = userData.family_name,
                        dob = userData.dob_ts,
                        id = userData.id,
                        qr_status = userData.qrcode_status ?? 0, // Handle possible null value
                        issue_ct = userData.issue_count,
                        test_ct = userData.test_count,
                        vaccine_status = userData.vaccine_status
                    };

                    return userDB;
                }
                else
                {
                    Console.WriteLine($"Error fetching user information: {userResponse.StatusCode} - {userResponse.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching user information: {ex.Message}");
                return null;
            }
        }

        // Class to represent the data received from the backend
        public class UserDataFromBackend
        {
            public string token { get; set; }
            public string id { get; set; }
            public string email { get; set; }
            public string family_name { get; set; }
            public string given_name { get; set; }
            public int dob_ts { get; set; }
            public int? qrcode_status { get; set; }
            public int issue_count { get; set; }
            public int test_count { get; set; }
            public int vaccine_status { get; set; }
        }

        public async Task<UserDB> CreateAccountAsync(string email, string password, string firstName, string lastName, int dob, string nhiNum)
        {
            try
            {
                // Create a string containing the user data
                UserData userData = new UserData
                {
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dob,
                    NhiNumber = nhiNum
                };

                // Convert the data to a JSON string using JsonConvert.SerializeObject
                string jsonString = JsonConvert.SerializeObject(userData);

                // Convert the JSON string to StringContent
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // Make a POST request to create a new account
                HttpResponseMessage response = await _client.PostAsync("https://cse106-backend.d3rpp.dev/auth/sign-up", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it into UserDB
                    string responseContent = await response.Content.ReadAsStringAsync();
                    UserDB user = JsonConvert.DeserializeObject<UserDB>(responseContent);

                    // Account creation successful, return the UserDB instance
                    return user;
                }
                else
                {
                    Console.WriteLine($"Error creating account: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }


        public class UserData // stroage for data generated in create account to be shipped to backend
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int DateOfBirth { get; set; }
            public string NhiNumber { get; set; }
        }

        public async Task RequestQRCodeAsync()
        {
            try
            {
                // Make a POST request to request a QR code
                HttpResponseMessage response = await _client.PostAsync("https://cse106-backend.d3rpp.dev/api/qrcodes/request", null);

                if (response.IsSuccessStatusCode)
                {
                    // QR code request successful
                    Console.WriteLine("QR code request successful.");
                }
                else
                {
                    Console.WriteLine($"Error requesting QR code: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task<List<UserDB>> GetUsersWithQRStatusAsync()
        {
            try
            {
                // Make a GET request to fetch users with qr_status = 1
                HttpResponseMessage response = await _client.GetAsync("https://cse106-backend.d3rpp.dev/api/qrcodes/requests");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to obtain the list of users
                    string content = await response.Content.ReadAsStringAsync();
                    List<UserDB> usersWithQR = JsonConvert.DeserializeObject<List<UserDB>>(content);

                    return usersWithQR;
                }
                else
                {
                    Console.WriteLine($"Error fetching users with QR status: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching users with QR status: {ex.Message}");
                return null;
            }
        }

                public async Task<HttpResponseMessage> ApproveQRCodeAsync(string userId)
                {
                    try
                    {
                        // Construct the URL for the PUT request
                        string apiUrl = $"https://cse106-backend.d3rpp.dev/api/qrcodes/approve/{userId}";

                        // Update the QR status to 2
                        var updateStatus = new
                        {
                            qr_status = 2
                        };

                        // Convert the data to StringContent
                        var stringContent = new StringContent(JsonConvert.SerializeObject(updateStatus), Encoding.UTF8, "application/json");

                        // Make a PUT request to update the QR status
                        HttpResponseMessage response = await this._client.PutAsync(apiUrl, stringContent);

                        return response;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while approving QR code: {ex.Message}");
                        return null;
                    }
                }

        public async Task<string> RetrieveQRCodeImageURLAsync(string userId)
        {
            try
            {
                // Construct the URL for the GET request including the userId
                string apiUrl = $"https://cse106-backend.d3rpp.dev/api/qrcodes/{userId}";

                // Make a GET request to retrieve the QR code image URL
                HttpResponseMessage response = await this._client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize the string to a string (assuming the URL is a string)
                    string qrCodeUrl = JsonConvert.DeserializeObject<string>(content);

                    return qrCodeUrl;
                }
                else
                {
                    Console.WriteLine($"Error retrieving QR code URL: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving QR code URL: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]> RetrieveQRCodeImageAsync(string imageUrl)
        {
            try
            {
                // Make a GET request to retrieve the QR code image
                HttpResponseMessage response = await _client.GetAsync(imageUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a byte array
                    byte[] imageData = await response.Content.ReadAsByteArrayAsync();

                    return imageData;
                }
                else
                {
                    Console.WriteLine($"Error retrieving QR code image: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving QR code image: {ex.Message}");
                return null;
            }
        }


        public async Task<List<Issue>> GetOpenIssuesAsync()
        {
            try
            {
                // Construct the URL for the GET request
                string apiUrl = "https://cse106-backend.d3rpp.dev/api/issues";

                // Make a GET request to retrieve all open issues
                HttpResponseMessage response = await this._client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON content to a list of Issue objects
                    List<Issue> openIssues = JsonConvert.DeserializeObject<List<Issue>>(jsonContent);
                    return openIssues;
                }
                else
                {
                    Console.WriteLine($"Error retrieving open issues: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving open issues: {ex.Message}");
                return null; // handle the error
            }
        }


        public async Task ReportIssueAsync(string subject, string description)
        {
            try
            {
                // Create a string containing the issue data
                Issue issueData = new Issue
                {
                    subject = subject,
                    description = description
                };

                // Convert the data to StringContent
                var stringContent = new StringContent(JsonConvert.SerializeObject(issueData), Encoding.UTF8, "application/json");

                // Make a POST request to report a new issue
                HttpResponseMessage response = await _client.PostAsync("https://cse106-backend.d3rpp.dev/api/issues/create", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    // Issue reporting successful
                    Console.WriteLine("Issue reported successfully!");
                }
                else
                {
                    Console.WriteLine($"Error reporting issue: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task<bool> CloseIssueInBackendAsync(string issueId)
        {
            try
            {
                // Prepare the URL for closing the issue
                string apiUrl = $"https://cse106-backend.d3rpp.dev/api/issues/{issueId}/close";

                // Make a PUT request to close the issue
                HttpResponseMessage response = await _client.PutAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    // Issue closure successful
                    Console.WriteLine($"Issue with ID {issueId} closed in the backend.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to close issue with ID {issueId} in the backend: {response.StatusCode} - {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteIssueInBackend(string issueId)
        {
            try
            {
                // Make a DELETE request to the backend to delete the issue
                HttpResponseMessage response = await _client.DeleteAsync($"https://cse106-backend.d3rpp.dev/api/issues/{issueId}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Issue with ID {issueId} deleted in the backend successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to delete issue with ID {issueId} in the backend: {response.StatusCode} - {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<Issue> GetIssueByIDAsync(string issueId)
        {
            try
            {
                // Make a GET request to the backend to retrieve the issue by ID
                HttpResponseMessage response = await _client.GetAsync($"https://cse106-backend.d3rpp.dev/api/issues/{issueId}");

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content and deserialize it into an Issue object
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Issue retrievedIssue = JsonConvert.DeserializeObject<Issue>(responseContent);

                    Console.WriteLine($"Issue with ID {issueId} retrieved successfully.");
                    return retrievedIssue;
                }
                else
                {
                    Console.WriteLine($"Error retrieving issue with ID {issueId}: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> AddVaccinationAsync(Vaccine firstDose, Vaccine secondDose)
        {
            try
            {
                // Construct the URL for the POST request
                string apiUrl = "https://cse106-backend.d3rpp.dev/api/vaccinations/add";

                // Create an object containing the vaccination data
                var vaccinationData = new
                {
                    first_dose = firstDose,
                    second_dose = secondDose
                };

                // Convert the data to StringContent
                var stringContent = new StringContent(JsonConvert.SerializeObject(vaccinationData), Encoding.UTF8, "application/json");

                // Make a POST request to add vaccination details
                HttpResponseMessage response = await _client.PostAsync(apiUrl, stringContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding vaccination details: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Vaccine>> GetVaccinationsAsync()//Admin Call
        {
            try
            {
                // Construct the URL for the GET request
                string apiUrl = "https://cse106-backend.d3rpp.dev/api/vaccinations/01HFMP3S8V";

                // Make a GET request to retrieve the vaccination information
                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize the string to a List<Vaccine> object
                    List<Vaccine> vaccinationData = JsonConvert.DeserializeObject<List<Vaccine>>(content);

                    return vaccinationData;
                }
                else
                {
                    Console.WriteLine($"Error retrieving vaccination information: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving vaccination information: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Vaccine>> GetUserVaccinationsAsync()//User Call
        {
            try
            {
                // Construct the URL for the GET request
                string apiUrl = "https://cse106-backend.d3rpp.dev/api/vaccinations";

                // Make a GET request to retrieve the vaccination information
                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize the string to a List<Vaccine> object
                    List<Vaccine> vaccinationData = JsonConvert.DeserializeObject<List<Vaccine>>(content);

                    return vaccinationData;
                }
                else
                {
                    Console.WriteLine($"Error retrieving vaccination information: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving vaccination information: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ReportTestAsync(CovidTest testReport)
        {
            try
            {
                // Construct the URL for the POST request
                string apiUrl = "https://cse106-backend.d3rpp.dev/api/tests/add";

                // Convert the test report data to StringContent
                var stringContent = new StringContent(JsonConvert.SerializeObject(testReport), Encoding.UTF8, "application/json");

                // Make a POST request to report the test details
                HttpResponseMessage response = await _client.PostAsync(apiUrl, stringContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reporting the test: {ex.Message}");
                return false;
            }
        }



        // Dispose method to clean up resources when the application exits
        public void Dispose()
        {
            this._client.Dispose();
        }
    }
}
