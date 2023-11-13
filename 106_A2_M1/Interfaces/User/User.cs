namespace _106_A2_M1.Interfaces.User
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Serialisation")]
    public class User : ResponseBody<User>
    {
        /// <summary>
        /// The ID of the user following the ULID standard
        /// <para>
        /// For more info on ULIDs, see <see href="https://github.com/ulid/spec"/>
        /// </para>
        /// </summary>
        public string id { get; }
        /// <summary>
        /// the e-mail address of the user, can be changed
        /// </summary>
        public string email { get; }
        /// <summary>
        /// The family name of the user, named as such since in some 
        /// languages and cultures the family name comes before the 
        /// given name and not after like it does in english, as such 
        /// we use this term instead of last name
        /// </summary>
        public string family_name { get; }
        /// <summary>
        /// The given name of the user
        /// </summary>
        public string given_name { get; }
        /// <summary>
        /// The users NHI Number
        /// </summary>
        public string national_health_index { get; }
        /// <summary>
        /// The users birthdate formatted as a UTC timestamp (milliseconds)
        /// 
        /// <para>
        /// The following stack overflow article should help in turning it into a 
        /// <see cref="System.DateTime"/> object <see href="https://stackoverflow.com/questions/17317466/c-sharp-how-to-convert-timestamp-to-date"/>
        /// </para>
        /// </summary>
        public int dob_ts { get; }
        /// <summary>
        /// <para>
        /// Key of what this number means
        /// </para>
        /// <list type="bullet">
        ///     <item>
        ///         <description><c>0</c> = Not Eligible for a Vaccination Certificate Request</description>
        ///     </item>
        ///     <item>
        ///         <description><c>1</c> = Eligible for a Vaccination Certificate but has not requested</description>
        ///     </item>
        ///     <item>
        ///         <description><c>2</c> = Eligible for a Vaccination Certificate and has requested</description>
        ///     </item>
        ///     <item>
        ///         <description><c>3</c> = A Vaccination Certificate (QR Code) is available for this user</description>
        ///     </item>
        /// </list>
        /// </summary>
        public int qrcode_status { get; }
        /// <summary>
        /// The number of issues that a user has submitted, this will be 0 initially on sign up
        /// </summary>
        public int issue_count { get; }
        /// <summary>
        /// The number of tests submitted by the user, this will be 0 initially on sign up
        /// </summary>
        public int test_count { get; }
        /// <summary>
        /// The vaccination status of the user, until the user gets vaccinated and an admin updates their account, this will be 0
        /// </summary>
        public int vaccine_status { get; }
    }
}

