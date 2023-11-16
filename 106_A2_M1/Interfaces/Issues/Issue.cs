namespace _106_A2_M1.Interfaces.Issues
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Deserialisation")]
    public class Issue : ResponseBody<Issue>
    {
        /// <summary>
        /// Unique ULID of the Issue
        /// 
        /// <para>
        /// see <see cref="_106_A2_M1.Interfaces.Auth.UserAuth.id"/> for more info about ULIDs
        /// </para>
        /// </summary>
        public string id {  get; set; }
        /// <summary>
        /// The id of the user that submitted the issue
        /// 
        /// <para>
        /// This is the id that can also be found in the body of <see cref="_106_A2_M1.Interfaces.Auth.UserAuth"/>
        /// </para>
        /// </summary>
        public string user_id { get; set; }

        public string subject { get; set; }
        public string description { get; set; }
        /// <summary>
        /// This is a timestamp in milliseconds from which the request was received to create an issue
        /// <para>
        /// see <see cref="_106_A2_M1.Interfaces.Auth.UserAuth.dob_ts"/> for more information on timestamps
        /// </para>
        /// </summary>
        public int opened_ts { get; set; }
        /// <summary>
        /// If this is <c>0</c> the issue is opened, if this is greater than <see cref="opened_ts"/> the issue is closed
        /// <para>
        /// see <see cref="_106_A2_M1.Interfaces.Auth.UserAuth.dob_ts"/> for more information on timestamps
        /// </para>
        /// </summary>
        public int closed_ts { get; set; }

    }
}
