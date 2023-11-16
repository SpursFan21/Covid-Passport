namespace _106_A2_M1.Interfaces.Auth
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Deserialisation")]
    public class AdminAuth: ResponseBody<AdminAuth>
    {
        /// <summary>
        /// The token for an admin account
        /// </summary>
        public string token { get; set; }
    }
}
