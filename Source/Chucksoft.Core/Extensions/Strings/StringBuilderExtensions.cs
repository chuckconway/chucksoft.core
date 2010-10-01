using System.Text;

namespace Chucksoft.Core.Extensions.Strings
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Removes trailling spaces from the stringbuilder + 1 character
        /// </summary>      
        /// <example>
        /// StringBuilder sb = new StringBuilder();
        /// foreach(User user in users)
        /// {
        ///    sb.Append(user.Id);
        ///    sb.Append(", ");
        /// }
        /// return sb.Strip(); //this will strip the trailing ", "
        /// </example>
        /// <returns>
        /// Returns the string contents of the trimmed StringBuilder
        /// </returns>      
        public static string Strip(this StringBuilder sb)
        {
            for (int i = sb.Length-1; i >= 0 && sb[i] == ' '; --i)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// Removes the specified number of characters from the end of the string
        /// </summary>      
        /// <example>
        /// StringBuilder sb = new StringBuilder();
        /// foreach(User user in users)
        /// {
        ///    sb.Append(user.Id);
        ///    sb.Append("--");
        /// }
        /// return sb.Strip(2); //this will strip the trailing "--"
        /// </example>
        /// <returns>
        /// Returns the string contents of the trimmed StringBuilder
        /// </returns>  
        public static string Strip(this StringBuilder sb, int length)
        {
            if (length > sb.Length || length < 1)
            {
                return sb.ToString();
            }
            sb.Remove(sb.Length - length, length);         
            return sb.ToString();
        }
    }
}