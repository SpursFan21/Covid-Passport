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
        public string Password { get; set; }
        public UserDB UserDB { get; set; }

        public void RequestQR()
        {
            // Update qr_status to 1
            UserDB.qr_status = 1;

            // Add logic to send user info to the queue (TBC)
            SendUserInfoToQueue();

            // For demonstration purposes
            Console.WriteLine($"User requested QR. QrStatus: {UserDB.qr_status}");
        }

        private void SendUserInfoToQueue()
        {
            // Add logic to send user info to the queue (TBC)
            Console.WriteLine("Sending user info to the queue...");
        }
        public void UpdateUserDetails1(string currentPassword, string newPassword, string email, string firstName, string lastName)//TBC
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Current password, new password, email, first name, and last name cannot be null or empty.");
            }

            // Update the basic details
            Email = email;
            FirstName = firstName;
            LastName = lastName;

            // Validate current password (replace this with your actual validation logic)
            if (!ValidateCurrentPassword(currentPassword))
            {
                throw new InvalidOperationException("Invalid current password.");
            }

            //ship to backend TBC

            // Custom logic for updating the password in the User class
            Password = newPassword;
            Console.WriteLine($"Updated password for email: {Email}");
        }

        private bool ValidateCurrentPassword(string currentPassword)
        {
            // Replace this with actual password validation logic
            return Password == currentPassword;
        }
        internal static void SetPassword(string value)
        {
            throw new NotImplementedException();
        }
    }
}
