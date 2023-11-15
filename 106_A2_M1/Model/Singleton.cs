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


        public async Task<int> LoginAsync(string email, string password, string authToken)
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
        private async Task<UserDB> GetUserInformationAsync()
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
            public string dob_ts { get; set; }
            public int? qrcode_status { get; set; }
            public int issue_count { get; set; }
            public int test_count { get; set; }
            public int vaccine_status { get; set; }
        }

        public async Task<int> CreateAccountAsync(string email, string password, string firstName, string lastName, int dob, int nhiNumber)
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
                    NhiNumber = nhiNumber
                };

                // Convert the data to StringContent
                var stringContent = new StringContent(userData.ToJSONString(), Encoding.UTF8, "application/json");

                // Make a POST request to create a new account
                HttpResponseMessage response = await _client.PostAsync("https://cse106-backend.d3rpp.dev/auth/sign-up", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    // Account creation successful
                    return 1;
                }
                else
                {
                    Console.WriteLine($"Error creating account: {response.StatusCode} - {response.ReasonPhrase}");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }

        public class UserData
        {
            // Define properties for user data
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int DateOfBirth { get; set; }
            public int NhiNumber { get; set; }

            // Method to convert the object to a JSON string
            public string ToJSONString()
            {
                return JsonConvert.SerializeObject(this);
            }
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


        // Dispose method to clean up resources when the application exits
        public void Dispose()
        {
            this._client.Dispose();
        }
    }
}
