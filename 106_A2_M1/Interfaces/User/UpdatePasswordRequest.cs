namespace _106_A2_M1.Interfaces.User
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Serialisation")]
    public class UpdatePasswordRequest: RequestBody<UpdatePasswordRequest>
    {
        public string old_password { get; set; }
        public string new_password { get; set; }
    }
}
