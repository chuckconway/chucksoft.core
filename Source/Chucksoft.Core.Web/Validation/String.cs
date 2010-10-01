using System.Text.RegularExpressions;

namespace Chucksoft.Core.Web.Validation
{
    public static class String
    {
        /// <summary>
        /// Determines whether [contains invalid characters] [the specified val].
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns>
        /// 	<c>true</c> if [contains invalid characters] [the specified val]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsMetaCharacters(this string val)
        {
            var regex = new Regex("^[<>&]*$");
            bool isMatch = regex.IsMatch(val);

            return isMatch;
        }
    }
}