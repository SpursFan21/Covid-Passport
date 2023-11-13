using System.Collections.Generic;

namespace _106_A2_M1.Interfaces.Issues
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "JSON Deserialisation")]
    public class Issues : ResponseBody<Issues>
    {
        /// <summary>
        /// The number of results, this is equivalent to <see cref="List{T}.Count"/>
        /// </summary>
        public int count { get; }
        /// <summary>
        /// Ignore, this will always be <c>0</c>
        /// </summary>
        public int offset { get; }
        /// <summary>
        /// The list of results, iterate over this to get your issues
        /// </summary>
        public List<Issue> results { get; }
    }
}
