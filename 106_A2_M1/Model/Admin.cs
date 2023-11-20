using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace _106_A2_M1.Model
{
    public class Admin : BaseUser
    {
        public string user_id { get; set; }

        public List<User> user_list { get; set; }
        public List<Issue> issue_list { get; set; }
        public List<Issue> OpenIssues { get; set; }

        //field to store the users with QR status
        private List<UserDB> usersWithQR;

        // Property to store the QR code image link
        public string QrCodeImageUrl { get; private set; }

        public Admin()
        {
            user_list = new List<User>();
            issue_list = new List<Issue>();
            usersWithQR = new List<UserDB>(); // Initialize the usersWithQR list
        }

        public void UpdateUserDetails(string email, string firstName, string lastName, int dateOfBirth, string nhiNumber)
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

            // Additional logic for updating Admin-specific details
            UserDB.dob = dateOfBirth;
            UserDB.nhi_num = nhiNumber;

            // For demonstration purposes
            Console.WriteLine($"Updated details for email: {email}, Date of Birth: {dateOfBirth}, NHI Number: {nhiNumber}");
        }

        // Function to manage users
        public void ManageUser() //TBC
        {
            // Your user management logic to display users in the admin dashboard

            // Example: Display a list of users
            System.Collections.IList list = user_list;
            for (int i = 0; i < list.Count; i++)
            {
                UserDB user = (UserDB)list[i];
                Console.WriteLine($"User: {user.id}, Email: {user.email}");
            }
        }

        public void GetUserData()
        {
            //TBC
        }

        public void DeleteVaccine() //TBC
        {
           
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
            BaseUser selectedUser = user_list.FirstOrDefault(u => u.UserDB.id == selectedUserId);

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
            BaseUser selectedUser = user_list.FirstOrDefault(u => u.UserDB.id == selectedUserId);

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

        public async Task GetQRListAsync()
        {
            try
            {
                // Use SingletonClient to fetch a list of users with qr_status = 1
                List<UserDB> usersWithQR = await SingletonClient.Instance.GetUsersWithQRStatusAsync();

                if (usersWithQR != null && usersWithQR.Any())
                {
                    // Display details of users with QR status 1 which = requesting QR aproval
                    foreach (var user in usersWithQR)
                    {
                        Console.WriteLine($"User: {user.first_name} {user.last_name}");
                        Console.WriteLine($"Email: {user.email}");
                        Console.WriteLine($"Vaccine Status: {user.vaccine_status}");
                        Console.WriteLine("---------------");
                    }
                }
                else
                {
                    Console.WriteLine("No users found with QR status 1.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task ApproveQRCodeAsync(string userId)
        {
            try
            {
                // Use SingletonClient to approve the QR code through a PUT request
                HttpResponseMessage response = await SingletonClient.Instance.ApproveQRCodeAsync(userId);

                if (response != null && response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"QR Code for user with ID {userId} approved successfully.");
                    await RetrieveQRCodeImageURLAsync(userId);
                    await RetrieveQRCodeImageAsync(userId);
                }
                else
                {
                    Console.WriteLine($"Failed to approve QR Code for user with ID {userId}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public async Task<string> RetrieveQRCodeImageURLAsync(string userId)
        {
            try
            {
                // Use SingletonClient to retrieve the QR code image URL asynchronously
                string qrCodeImageURL = await SingletonClient.Instance.RetrieveQRCodeImageURLAsync(userId);

                if (qrCodeImageURL != null)
                {
                    Console.WriteLine($"QR Code Image URL for user with ID {userId}: {qrCodeImageURL}");
                    return qrCodeImageURL;
                }
                else
                {
                    Console.WriteLine($"QR Code Image URL not found for user with ID {userId}.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }


        public async Task RetrieveQRCodeImageAsync(string userId)
        {
            try
            {
                // Use SingletonClient to retrieve the QR code image URL asynchronously
                string qrCodeImageURL = await SingletonClient.Instance.RetrieveQRCodeImageURLAsync(userId);

                if (qrCodeImageURL != null)
                {
                    // Use SingletonClient to retrieve the QR code image
                    byte[] imageData = await SingletonClient.Instance.RetrieveQRCodeImageAsync(qrCodeImageURL);

                    if (imageData != null)
                    {
                        // Assuming you have a method to save the image, adjust accordingly
                        SaveQRCodeImage(imageData, userId);

                        Console.WriteLine($"QR Code Image for user with ID {userId} retrieved successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve QR Code Image for user with ID {userId}.");
                    }
                }
                else
                {
                    Console.WriteLine($"QR Code Image URL not found for user with ID {userId}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void SaveQRCodeImage(byte[] imageData, string userId)
        {
            try
            {
                if (imageData != null && imageData.Length > 0)
                {
                    // Implement logic to save the image data to a file
                    File.WriteAllBytes($"QRCode_{userId}.png", imageData);

                    Console.WriteLine($"QR code image for user with ID {userId} saved successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: Invalid or empty QR code image data for user with ID {userId}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving QR code image: {ex.Message}");
            }
        }


        public void DeleteQR(string selectedUserId) //TBC
        {
            // Find the selected user
            BaseUser selectedUser = user_list.FirstOrDefault(u => u.UserDB.id == selectedUserId);

            if (selectedUser != null)
            {
                // Call the backend to delete the QR code for the selected user

                // Mock response
                bool isDeleted = DeleteQRCodeFromBackend(selectedUser.UserDB.id);

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

        public async Task ViewAllOpenIssuesAsync()
        {
            try
            {
                // Use the SingletonClient to fetch all open issues from the backend
                List<Issue> openIssues = await SingletonClient.Instance.GetOpenIssuesAsync();

                if (openIssues != null && openIssues.Any())
                {
                    // Store the open issues for display purposes
                    this.OpenIssues = openIssues;

                    // Display details of open issues
                    foreach (var issue in openIssues)
                    {
                        Console.WriteLine($"Issue ID: {issue.issue_id}");
                        Console.WriteLine($"Subject: {issue.subject}");
                        Console.WriteLine($"Description: {issue.description}");
                        Console.WriteLine("---------------");
                    }
                }
                else
                {
                    Console.WriteLine("No open issues found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task CloseIssueAsync(string issueId)
        {
            try
            {
                // Find the issue in the issue list by issueId
                Issue selectedIssue = issue_list.FirstOrDefault(issue => issue.issue_id == issueId);

                if (selectedIssue != null)
                {
                    // Make a request to the backend to close the issue
                    bool isClosed = await CloseIssueInBackend(selectedIssue.issue_id);

                    if (isClosed)
                    {
                        // Update the issue's closed_date with Unix timestamp
                        selectedIssue.closed_date = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        Console.WriteLine($"Issue with ID {issueId} closed successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to close issue with ID {issueId} in the backend.");
                    }
                }
                else
                {
                    Console.WriteLine($"Issue with ID {issueId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public async Task<bool> CloseIssueInBackend(string issueId)
        {
            try
            {
                // Use SingletonClient to close the issue through a PUT request
                bool isClosed = await SingletonClient.Instance.CloseIssueInBackendAsync(issueId);

                return isClosed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task DeleteIssueAsync(string issueId)
        {
            try
            {
                // Find the issue in the issue list by issueId
                Issue selectedIssue = issue_list.FirstOrDefault(issue => issue.issue_id == issueId);

                if (selectedIssue != null)
                {
                    // Remove the selected issue from the list
                    issue_list.Remove(selectedIssue);

                    // Use SingletonClient to delete the issue through a DELETE request
                    await SingletonClient.Instance.DeleteIssueInBackend(issueId);

                    Console.WriteLine($"Issue with ID {issueId} deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Issue with ID {issueId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Issue> GetIssueByIDAsync(string issueId)
        {
            try
            {
                // Use SingletonClient to retrieve the issue by ID asynchronously
                Issue retrievedIssue = await SingletonClient.Instance.GetIssueByIDAsync(issueId);

                if (retrievedIssue != null)
                {
                    Console.WriteLine($"Found issue with ID {issueId}:");
                    Console.WriteLine($"Subject: {retrievedIssue.subject}");
                    Console.WriteLine($"Description: {retrievedIssue.description}");
                    Console.WriteLine($"Open Date: {retrievedIssue.open_date}");
                    Console.WriteLine($"Closed Date: {retrievedIssue.closed_date}");
                    Console.WriteLine($"Resolved: {retrievedIssue.resolve}");
                    return retrievedIssue;
                }
                else
                {
                    Console.WriteLine($"Issue with ID {issueId} not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task GetVaccinationsAsync()
        {
            try
            {
                // Use SingletonClient to get vaccination information through a GET request
                List<Vaccine> vaccinationData = await SingletonClient.Instance.GetVaccinationsAsync();

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
    }
}
