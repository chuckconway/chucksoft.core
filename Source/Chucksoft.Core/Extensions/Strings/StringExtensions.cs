using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Chucksoft.Core.Extensions.Strings
{
    public static class StringExtensions
    {
        private static readonly Regex _htmlTagPattern = new Regex(@"<\/?[^>]*>", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex _multipleSpaces = new Regex(@"\s{2,}", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex _noSpaceAfterPunctuation = new Regex(@"([\.\?\!,:;\-]""?)(\S)", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex _firstLowerCaseLetterAfterSentence = new Regex(@"(\.\s*)([a-z])", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex _lowercaseI = new Regex(@"([\s\p{P}])i([\s\p{P}])", RegexOptions.Compiled | RegexOptions.Multiline);


        /// <summary>
        /// Compares a string to a given string. The comparison is case insensitive.
        /// </summary>
        /// <param name="string">The @string.</param>
        /// <param name="compareTo">The string to compare against</param>
        /// <returns>
        /// True if the strings are the same, false otherwise.
        /// </returns>
        public static bool Is(this string @string, string compareTo)
        {
            return string.Compare(@string, compareTo, true) == 0;
        }

        /// <summary>
        /// Creates a type from the given name
        /// </summary>
        /// <typeparam name="T">The type being created</typeparam>      
        /// <param name="typeName"></param>
        /// <param name="args">Arguments to pass into the constructor</param>
        /// <returns>An instance of the type</returns>
        public static T CreateType<T>(this string typeName, params object[] args)
        {
            Type type = Type.GetType(typeName, true, true);
            return (T)Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Replaces each newline with a &lt;br /&gt; tag
        /// </summary>      
        public static string NewlineToBr(this string html)
        {
            return html.Replace(Environment.NewLine, "<br />");
        }

        /// <summary>
        /// Removes html tags from a given strnig
        /// </summary>      
        public static string StripHtml(this string html)
        {
            return _htmlTagPattern.Replace(html, "");
        }

        /// <summary>
        /// Removes extra spaces within a string
        /// </summary>      
        public static string Strip(this string @string)
        {
            return _multipleSpaces.Replace(@string, " ");
        }

        /// <summary>
        /// Fixes a paragraph so that it more properly conforms to english rules (single space after punctuation,
        /// capitalization and so on).
        /// </summary>      
        public static string Proper(this string paragraph)
        {
            paragraph = _noSpaceAfterPunctuation.Replace(paragraph, "$1 $2").Strip();
            paragraph = _firstLowerCaseLetterAfterSentence.Replace(paragraph, m => m.Groups[1].Value + m.Groups[2].Value.ToUpper());
            paragraph = paragraph.Substring(0, 1).ToUpper() + paragraph.Substring(1);
            paragraph = _lowercaseI.Replace(paragraph, "$1I$2");
            return paragraph;
        }

        /// <summary>
        /// Reverse the order of a string
        /// </summary>      
        public static string Reverse(this string @string)
        {
            var reversed = @string.ToCharArray();
            Array.Reverse(reversed);
            return new string(reversed);
        }

        /// <summary>
        /// Applies formatting to the specified string
        /// </summary>      
        public static string FormatWith(this string @string, params object[] args)
        {
            return string.Format(@string, args);
        }

        /// <summary>
        /// Capitalizes the first letter of a string
        /// </summary>      
        public static string Capitalize(this string @string)
        {
            if (@string.Length == 0)
            {
                return @string;
            }
            if (@string.Length == 1)
            {
                return @string.ToUpper(CultureInfo.InvariantCulture);
            }
            return @string.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + @string.Substring(1);
        }

        /// <summary>
        /// Returns the right portion of the string for the specified length
        /// </summary>
        public static string Right(this string @string, int length)
        {
            if (length <= 0 || @string.Length == 0)
            {
                return string.Empty;
            }
            if (@string.Length <= length)
            {
                return @string;
            }
            return @string.Substring(@string.Length - length, length);
        }

        /// <summary>
        /// Returns the left portion of the string for the specified length
        /// </summary>
        public static string Left(this string @string, int length)
        {
            if (length <= 0 || @string.Length == 0)
            {
                return string.Empty;
            }
            if (@string.Length <= length)
            {
                return @string;
            }
            return @string.Substring(0, length);
        }

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

        ///// <summary>
        ///// Convert the First Character in a string to uppercase, if it's null or empty, it's returned as such.
        ///// </summary>
        ///// <param name="capitalize">string to have first letter uppercased</param>
        ///// <returns>Capitalized string</returns>
        //public static string CapitalCase(this string capitalize)
        //{
        //    if (!string.IsNullOrEmpty(capitalize))
        //    {
        //        char[] firstChars = capitalize.ToCharArray();
        //        firstChars[0] = Convert.ToChar(firstChars[0].ToString().ToUpper());

        //        capitalize = new string(firstChars);
        //    }

        //    return capitalize;
        //}

        /// <summary>
        /// Serialize an object to a string.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns></returns>
        public static string SerializeToString(this object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }

        /// <summary>
        /// Deserialize an object from a string.
        /// </summary>
        /// <typeparam name="T">Type of destination object</typeparam>
        /// <param name="xml">Serialized XML source of an object</param>
        /// <returns></returns>
        public static T DeserializeFromString<T>(this string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Converts to base64 string.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string ConvertToBase64(this string val)
        {
            byte[] bytes = Encoding.Default.GetBytes(val);
            string basedString = Convert.ToBase64String(bytes);

            return basedString;
        }

        /// <summary>
        /// Froms the base64 string.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string FromBase64String(this string val)
        {
            byte[] bytes = Convert.FromBase64String(val);
            string cleanString = Encoding.Default.GetString(bytes);

            return cleanString;
        }

        /// <summary>
        /// Attempts to extract an integer from a string. This function behaves similarly
        /// to atoi functions, which will read up to a non-numeric value. For example:
        /// 103abc --> 103   or -1!  -> -1 
        /// </summary>
        /// <remarks>
        /// This will throw an exception on failure
        /// </remarks>      
        public static int ExtractInt(this string @string)
        {
            if (@string.Length == 0)
            {
                throw new FormatException();
            }
            @string = @string.Trim();
            bool isNegative = (@string[0] == '-');
            if (isNegative && @string.Length == 1)
            {
                throw new FormatException();
            }
            int offset = isNegative ? 1 : 0;
            if (@string[offset] < '0' || @string[offset] > '9')
            {
                throw new FormatException();
            }
            int value = 0;
            for (; offset < @string.Length; ++offset)
            {
                char c = @string[offset];
                if (c < '0' || c > '9')
                {
                    break;
                }
                value = value * 10 + (c - '0');
            }
            return isNegative ? -1 * value : value;
        }

        /// <summary>
        /// Attempts to extract an integer from a string
        /// </summary>      
        public static bool TryExtractInt(this string @string, out int value)
        {
            try
            {
                value = @string.ExtractInt();
                return true;
            }
            catch (FormatException)
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// Parses an integer from a string
        /// </summary>      
        public static int ToInt(this string @string)
        {
            return int.Parse(@string);
        }

        /// <summary>
        /// Parses a boolean from a string (including "0" and "1")
        /// </summary>      
        public static bool ToBoolean(this string @string)
        {
            if (@string == "0")
            {
                return false;
            }
            if (@string == "1")
            {
                return true;
            }
            return bool.Parse(@string);
        }

        /// <summary>
        /// Parses a double from a string
        /// </summary>      
        public static double ToDouble(this string @string)
        {
            return double.Parse(@string);
        }

        /// <summary>
        /// Parses a float from a string
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static float ToFloat(this string @string)
        {
            return float.Parse(@string);
        }

        /// <summary>
        /// Converts a string to bytes
        /// </summary>      
        public static byte[] ToBytes(this string @string)
        {
            return @string.ToBytes(EncodingType.Default);
        }

        /// <summary>
        /// Converts a string to bytes
        /// </summary>      
        public static byte[] ToBytes(this string @string, EncodingType encoding)
        {
            return GetEncoding(encoding).GetBytes(@string);
        }

        /// <summary>
        /// Gets the encoding.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static Encoding GetEncoding(EncodingType encoding)
        {
            switch (encoding)
            {
                case EncodingType.Utf8:
                    return Encoding.UTF8;
                case EncodingType.Utf7:
                    return Encoding.UTF7;
                case EncodingType.Unicode:
                    return Encoding.Unicode;
                case EncodingType.Ascii:
                    return Encoding.ASCII;
                default:
                    return Encoding.Default;
            }
        }
    }

    public enum EncodingType
    {
        Utf7,
        Utf8,
        Ascii,
        Unicode,
        Default,
    }

}
