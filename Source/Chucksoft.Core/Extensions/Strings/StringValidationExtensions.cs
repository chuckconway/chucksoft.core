
using System.Reflection;
using System.Text.RegularExpressions;

namespace Chucksoft.Core.Extensions.Strings
{
    public static class StringValidationExtensions
    {
        private static readonly Regex _emailPattern = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,6}|[0-9]{1,})(\]?)$", RegexOptions.Compiled);

        /// <summary>
        /// Validates that a string is a syntactically valid email address
        /// </summary>
        /// <param name="email">The string to validate</param>
        /// <returns>True if the string is a valid email, false otherwise</returns>
        public static bool IsEmail(this string email)
        {
            return _emailPattern.IsMatch(email);
        }

        /// <summary>
        /// Determines whether a word is potentially a bad word
        /// </summary>
        /// <remarks>
        /// The bad-word list comes form an embedded resource, modifications can be made by getting 
        /// the source and changing BadWordList.txt.
        /// 
        /// Matches are intentionally kept loose. This means "afuck" won't be considered a bad word,
        /// it also means Shelly (hello) or Bass (ass) won't be considered a bad word. This might be
        /// tweaked int he future.
        /// </remarks>      
        /// <returns>True if the word is a bad word, false otherwise</returns>
        public static bool IsBadWord(this string word)
        {
            var patterns = Assembly.GetExecutingAssembly().GetManifestResourceStream("CodeBetter.Extensions.Strings.BadWordList.txt").ReadLines();
            foreach (string pattern in patterns)
            {
                if (Regex.IsMatch(word, "^" + pattern + "$", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}