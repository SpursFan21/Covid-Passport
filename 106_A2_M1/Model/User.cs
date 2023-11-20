using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class User : BaseUser
    {
        private List<Issue> userIssues;

        public async Task RequestQRAsync()
        {
            // Update qr_status to 1 which means they want QR aproval from Admin
            UserDB.qr_status = 1;

            // Use SingletonClient to request a QR code and send user to QR que
            await SingletonClient.Instance.RequestQRCodeAsync();

            // For demonstration purposes
            Console.WriteLine($"User requested QR. QrStatus: {UserDB.qr_status}");
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
        public async Task AddFirstDose(string doseId, int dateAdministered, string brand, string location)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(doseId) || dateAdministered <= 0 || string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(location))
                {
                    throw new ArgumentException("Invalid first dose details. Please provide valid information.");
                }

                // Create a new first dose
                first_dose = new Vaccine
                {
                    dose_id = doseId,
                    date_administered = dateAdministered,
                    brand = brand,
                    location = location
                };

                // Use SingletonClient to add the first dose details through a POST request
                bool isAdded = await SingletonClient.Instance.AddVaccinationAsync(first_dose, null);

                if (isAdded)
                {
                    // Perform any additional logic for the first dose addition
                    Console.WriteLine("First dose added successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to add the first dose to the backend.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task AddSecondDose(string doseId, int dateAdministered, string brand, string location)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(doseId) || dateAdministered <= 0 || string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(location))
                {
                    throw new ArgumentException("Invalid second dose details. Please provide valid information.");
                }

                // Create a new second dose
                second_dose = new Vaccine
                {
                    dose_id = doseId,
                    date_administered = dateAdministered,
                    brand = brand,
                    location = location
                };

                // Use SingletonClient to add the second dose details through a POST request
                bool isAdded = await SingletonClient.Instance.AddVaccinationAsync(null, second_dose);

                if (isAdded)
                {
                    // Perform any additional logic for the second dose addition
                    Console.WriteLine("Second dose added successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to add the second dose to the backend.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


    }
}