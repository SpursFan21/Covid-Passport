namespace _106_A2_M1.Interfaces.Issues
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Serialisation")]
    public class CreateIssue: RequestBody<CreateIssue>
    {
        public string subject { get; set; }
        public string description { get; set; }
    }
}
