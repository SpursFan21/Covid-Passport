using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _106_A2_M1.Model
{
    public class Admin : BaseUser
    {
        public string user_id { get; set; }

        public List<User> user_list { get; set; }
        public List<Issue> issue_list { get; set; }

        public override void UpdateUserDetails(string email, string firstName, string lastName, int dateOfBirth, int nhiNumber)//TBC
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

            // Additional logic for updating Admin-specific details
            DateOfBirth = dateOfBirth;
            NhiNumber = nhiNumber;

            // For demonstration purposes
            Console.WriteLine($"Updated details for email: {Email}, Date of Birth: {DateOfBirth}, NHI Number: {NhiNumber}");
        }

        public void ManageUser()
        {
            // Your user management logic to display users in the admin dashboard

            // Example: Display a list of users
            foreach (BaseUser user in user_list)
            {
                Console.WriteLine($"User: {user.id}, Email: {user.Email}");
            }
        }

        public void DeleteVaccine()
        {
            // Allow the admin to select a user and perform action
            Console.WriteLine("Enter the user ID to manage:");
            string selectedUserId = Console.ReadLine();

            // Find the selected user
            User selectedUser = user_list.FirstOrDefault(u => u.id == selectedUserId);

            if (selectedUser != null)
            {
                // Example: Call the deleteVaccination function for the selected user
                Console.WriteLine("Enter the dose ID to delete:");
                string doseIdToDelete = Console.ReadLine();

                Console.WriteLine("Enter the dose type to delete (first/second):");
                string doseTypeToDelete = Console.ReadLine();

                selectedUser.deleteVaccination(doseIdToDelete, doseTypeToDelete);
            }
            else
            {
                Console.WriteLine($"User with ID {selectedUserId} not found.");
            }
        }


        /*private BaseUser FindUser(string userId)TBC
        {
            // Sample implementation: 
            return Database.Users.FirstOrDefault(u => u.Id == userId);
        }
        */
        public void AddVaccination()
        {
            // Allow the admin to select a user to add a vaccination
            Console.WriteLine("Enter the user ID to manage:");
            string selectedUserId = Console.ReadLine();

            // Find the selected user
            BaseUser selectedUser = user_list.FirstOrDefault(u => u.id == selectedUserId);

            if (selectedUser != null)
            {
               
                Console.WriteLine("Enter the dose ID:");
                string doseId = Console.ReadLine();

                Console.WriteLine("Enter the date administered:");
                int dateAdministered = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter the brand:");
                string brand = Console.ReadLine();

                Console.WriteLine("Enter the location:");
                string location = Console.ReadLine();

                // Determine which dose the admin wants to add
                Console.WriteLine("Which dose do you want to add? Enter 'first' or 'second':");
                string doseType = Console.ReadLine();

                // Create a new Vaccine instance
                Vaccine newVaccine = new Vaccine
                {
                    dose_id = doseId,
                    date_administered = dateAdministered,
                    brand = brand,
                    location = location
                };

                // Assign the new vaccine to the specified dose
                if (doseType.ToLower() == "first")
                {
                    selectedUser.first_dose = newVaccine;
                }
                else if (doseType.ToLower() == "second")
                {
                    selectedUser.second_dose = newVaccine;
                }
                else
                {
                    Console.WriteLine("Invalid dose type.");
                    return;
                }

                Console.WriteLine($"Added {doseType} dose for user with ID {selectedUserId}");
            }
            else
            {
                Console.WriteLine($"User with ID {selectedUserId} not found.");
            }
        }
        public void DeleteTest()
        {
            // Allow the admin to select a user to manage tests
            Console.WriteLine("Enter the user ID to manage tests:");
            string selectedUserId = Console.ReadLine();

            // find selected user
            BaseUser selectedUser = user_list.FirstOrDefault(u => u.id == selectedUserId);

            if (selectedUser != null)
            {
                // Display the list of tests for the selected user
                Console.WriteLine($"Tests for user with ID {selectedUserId}:");
                foreach (CovidTest test in selectedUser.test_list)
                {
                    Console.WriteLine($"Test ID: {test.test_id}, Date: {test.test_date}, Result: {test.result}, Type: {test.test_type}");
                }

                // Take input for the test ID to delete
                Console.WriteLine("Enter the test ID to delete:");
                string testIdToDelete = Console.ReadLine();

                // Find the test in the user's test_list
                CovidTest testToDelete = selectedUser.test_list.FirstOrDefault(t => t.test_id == testIdToDelete);

                if (testToDelete != null)
                {
                    // Remove the test from the user's test_list
                    selectedUser.test_list.Remove(testToDelete);
                    Console.WriteLine($"Deleted test with ID {testIdToDelete} for user with ID {selectedUserId}");
                }
                else
                {
                    Console.WriteLine($"Test with ID {testIdToDelete} not found for user with ID {selectedUserId}");
                }
            }
            else
            {
                Console.WriteLine($"User with ID {selectedUserId} not found.");
            }
        }

        public void GenerateQR()
        {
            // Allow the admin to select a user to generate a QR code
            Console.WriteLine("Enter the user ID to generate QR code:");
            string selectedUserId = Console.ReadLine();

            // Find the selected user
            BaseUser selectedUser = user_list.FirstOrDefault(u => u.id == selectedUserId);

            if (selectedUser != null)
            {
                // Call the backend to generate QR code for the selected user TBC
                // make an HTTP request to the backend here TBC
                // and receive the image link for the generated QR code. TBC

                /* Mock response for demonstration purposes
                string mockQRCodeImageLink = "https://example.com/qrcode/123456";
                selectedUser.image_link = mockQRCodeImageLink;
                

                Console.WriteLine($"QR code generated for user with ID {selectedUserId}");
                Console.WriteLine($"QR code image link: {mockQRCodeImageLink}");

                // Example: Open the QR code image link in the view
                // This could involve opening a browser window or displaying it in your application's view.
                // The specific implementation depends on your application's architecture.
                */
            }
            else
            {
                Console.WriteLine($"User with ID {selectedUserId} not found.");
            }
        }
        public void DeleteQR()
        {
            // Allow the admin to select a user to delete the QR code
            Console.WriteLine("Enter the user ID to delete QR code:");
            string selectedUserId = Console.ReadLine();

            // Find the selected user
            BaseUser selectedUser = user_list.FirstOrDefault(u => u.id == selectedUserId);

            if (selectedUser != null)
            {
                // Example: Call the backend to delete the QR code for the selected user
                // You would typically make an HTTP request to the backend here.
                // The specific API endpoint and payload depend on your backend implementation.

                // Mock response
                bool isDeleted = DeleteQRCodeFromBackend(selectedUser.id);

                if (isDeleted)
                {
                    // Reset the user's image link to null or an appropriate default value
                    selectedUser.image_link = null;

                    Console.WriteLine($"QR code deleted for user with ID {selectedUserId}");
                }
                else
                {
                    Console.WriteLine($"Failed to delete QR code for user with ID {selectedUserId}");
                }
            }
            else
            {
                Console.WriteLine($"User with ID {selectedUserId} not found.");
            }
        }

        private bool DeleteQRCodeFromBackend(string userId)
        {
            // Mock backend response indicating success or failure
            // Replace this TBC
            return true;
        }

        public void DenyQR()
        {
            // Example: Allow the admin to select a user to deny the QR code
            Console.WriteLine("Enter the user ID to deny QR code:");
            string selectedUserId = Console.ReadLine();

            // Find the selected user
            BaseUser selectedUser = user_list.FirstOrDefault(u => u.id == selectedUserId);

            if (selectedUser != null)
            {
                // Example: Call the backend to deny the QR code for the selected user
                // You would typically make an HTTP request to the backend here.
                // The specific API endpoint and payload depend on your backend implementation.

                // Mock response for demonstration purposes
                bool isDenied = DenyQRCodeInBackend(selectedUser.id);

                if (isDenied)
                {
                    // Update the user's qr_status to 0
                    selectedUser.db_member.qr_status = 0;

                    Console.WriteLine($"QR code denied for user with ID {selectedUserId}");
                }
                else
                {
                    Console.WriteLine($"Failed to deny QR code for user with ID {selectedUserId}");
                }
            }
            else
            {
                Console.WriteLine($"User with ID {selectedUserId} not found.");
            }
        }

        private bool DenyQRCodeInBackend(string userId)
        {
            // Mock backend response indicating success or failure
            // Replace this with actual logic to deny the QR code in backend TBC
            return true;
        }
        public void searchDirectory()
        {
            // come back another time TBC
        }
        public void searchQR()
        {
            // come back another time TBC
        }
        public void searchIssue()
        {
            // come back another time TBC
        }

        public void UpdateIssue()
        {
            Console.WriteLine("Enter the issue ID to update:");
            string issueIdToUpdate = Console.ReadLine();

            // Find the issue
            Issue selectedIssue = issue_list.FirstOrDefault(issue => issue.issue_id == issueIdToUpdate);

            if (selectedIssue != null)
            {
                // Update the resolve status
                selectedIssue.resolve = true;

                Console.WriteLine($"Issue with ID {issueIdToUpdate} has been resolved.");
            }
            else
            {
                Console.WriteLine($"Issue with ID {issueIdToUpdate} not found.");
            }
        }


    }
}
