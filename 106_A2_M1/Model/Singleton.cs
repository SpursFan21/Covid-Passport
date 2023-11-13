using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using _106_A2_M1.Interfaces.Auth;

namespace _106_A2_M1.Model
{
    public class SingletonClient
    {
        private static SingletonClient instance;

        private HttpClient _client;

        private SingletonClient()
        {
            
        }
        public static SingletonClient Instance
        {
            get
            {
                if (instance == null)
                    instance = new SingletonClient();
                return instance;
            }
        }

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
                        // not admin
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




        public async Task PopulateUserDataAsync()
        {

        }

        // Dispose method to clean up resources when the application exits
        public void Dispose()
        {
            this._client.Dispose();
        }
    }
}
