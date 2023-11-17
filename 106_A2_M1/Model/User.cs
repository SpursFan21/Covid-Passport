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
            Password = newPassword;
            Console.WriteLine($"Updated password for email: {email}");
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
