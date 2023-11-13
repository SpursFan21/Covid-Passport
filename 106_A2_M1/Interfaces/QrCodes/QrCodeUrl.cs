namespace _106_A2_M1.Interfaces.QrCodes
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Deserialisation")]
    public class QrCodeUrl
    {
        /// <summary>
        /// the URL for the image
        /// 
        /// add this in a form control for remote images
        /// </summary>
        public string url { get; }
        /// <summary>
        /// Expiry date as a timestamp, in milliseconds
        /// 
        /// <para>
        /// The following stack overflow article should help in turning it into a  <see cref="System.DateTime"/> object 
        /// <see href="https://stackoverflow.com/questions/17317466/c-sharp-how-to-convert-timestamp-to-date"/>
        /// </para>
        /// </summary>
        public int exp {  get; }
    }
}
