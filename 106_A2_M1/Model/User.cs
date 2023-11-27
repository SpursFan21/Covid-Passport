using _106_A2_M1.Interfaces.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class User : BaseUser
    {
        private List<Issue> userIssues;


        public async Task RequestQRCodeAsync()
        {
            try
            {
                // Update qr_status to 1, indicating the user wants QR approval from Admin
                UserDB.qr_status = 1;

                // Use SingletonClient to make a POST request to request a QR code
                HttpResponseMessage response = await SingletonClient.Instance.RequestQRCodeAsync();

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

        public void UpdateUserDetails1(string currentPassword, string newPassword, string email, string firstName, string lastName)//TBC
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Current password, new password, email, first name, and last name cannot be null or empty.");
            }

            // Update the basic details
            UserDB.email = email;
            UserDB.first_name = firstName;
            UserDB.last_name = lastName;

            // Validate current password (replace this with your actual validation logic)
            if (!ValidateCurrentPassword(currentPassword))
            {
                throw new InvalidOperationException("Invalid current password.");
            }

            //ship to backend TBC

            // Custom logic for updating the password in the User class
            password = newPassword;
            Console.WriteLine($"Updated password for email: {email}");
        }

        private bool ValidateCurrentPassword(string currentPassword)
        {
            // Replace this with actual password validation logic
            return password == currentPassword;
        }
        internal static void SetPassword(string value)
        {
            throw new NotImplementedException();
        }

        public async Task ReportIssueAsync(string subject, string description)
        {
            try
            {
                // Assuming you have a list of issues in your User class
                if (userIssues == null)
                {
                    userIssues = new List<Issue>();
                }

                // Create a new issue with subject and description
                Issue newIssue = new Issue
                {
                    subject = subject,
                    description = description
                };

                // Add the new issue to the list
                userIssues.Add(newIssue);

                // Use SingletonClient to report the issue through a POST request
                await SingletonClient.Instance.ReportIssueAsync(subject, description);

                Console.WriteLine("Issue reported successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task ReportTestAsync(string testId, int testDate, bool result, string testType)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(testId) || testDate <= 0 || string.IsNullOrEmpty(testType))
                {
                    throw new ArgumentException("Invalid test details. Please provide valid information.");
                }

                // Create a new CovidTest object
                CovidTest testReport = new CovidTest
                {
                    test_id = testId,
                    test_date = testDate,
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
        }

        public async Task<UserDB> GetOwnProfileAsync()
        {
            try
            {
                // Use SingletonClient to get the own user profile information through a GET request
                UserDB = await SingletonClient.Instance.GetOwnProfileAsync();

                if (UserDB != null)
                {
                    Console.WriteLine("Your Profile Information:");
                    Console.WriteLine($"User ID: {UserDB.id}");
                    Console.WriteLine($"Email: {UserDB.email}");
                    Console.WriteLine($"First Name: {UserDB.first_name}");
                    Console.WriteLine($"Last Name: {UserDB.last_name}");
                    Console.WriteLine($"Date of Birth: {UserDB.dob}");
                    Console.WriteLine($"NHI Number: {UserDB.nhi_num}");
                    Console.WriteLine($"QR Status: {UserDB.qr_status}");
                    Console.WriteLine($"Issue Count: {UserDB.issue_ct}");
                    Console.WriteLine($"Test Count: {UserDB.test_ct}");
                    Console.WriteLine($"Vaccine Status: {UserDB.vaccine_status}");
                }
                else
                {
                    Console.WriteLine("Failed to retrieve your profile from the backend.");
                }

                return UserDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }




    }
}