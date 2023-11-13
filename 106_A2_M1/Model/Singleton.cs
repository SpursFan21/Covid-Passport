using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

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

        public async Task LoginAsync(string email, string password, string authToken)
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
                string loginData = $"{{\"email\": \"{email}\", \"password\": \"{password}\"}}";

                // Convert the data to StringContent
                var stringContent = new StringContent(loginData, Encoding.UTF8, "application/json");

                // Add the Authorization header with the provided authToken
                this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

                // Make a POST request with the login data and Authorization header
                HttpResponseMessage response = await this._client.PostAsync(apiUrl, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
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
