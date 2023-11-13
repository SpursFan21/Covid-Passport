namespace _106_A2_M1.Interfaces.Auth
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Serialisation")]
    public class LoginRequest: RequestBody<LoginRequest>
    {
        /// <summary>
        /// Email Address to sign in
        /// </summary>
        public string email {  get; set; }
        /// <summary>
        /// Password in plain text to sign in
        /// </summary>
        public string password { get; set; }
    }
}
