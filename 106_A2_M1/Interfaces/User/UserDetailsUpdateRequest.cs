namespace _106_A2_M1.Interfaces.User
{
    /// <summary>
    /// This contains all the data the user can update or an admin, up to you really
    /// 
    /// <para>
    /// All data members must be supplied regardless of if its actually updated
    /// </para>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Serialisation")]
    public class UserDetailsUpdateRequest : RequestBody<UserDetailsUpdateRequest>
    {
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
    }
}
