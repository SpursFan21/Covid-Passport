namespace _106_A2_M1.Interfaces.Auth
{
    /// <summary>
    /// <para>
    /// This inherist a user class and receives all of the same properties and functions, it does not directly inherit 
    /// <see cref="_106_A2_M1.Interfaces.ResponseBody{T}"/> But it inherits it since <see cref="_106_A2_M1.Interfaces.User.User"/> 
    /// inherits it.
    /// </para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Deserialisation")]
    public class UserAuth : User.User
    {
        /// <summary>
        /// <para>
        /// The token that should be put into the HTTP Client Headers in order to Authenticate all requests from now on
        /// </para>
        /// </summary>
        public string token { get; }
    }
}
