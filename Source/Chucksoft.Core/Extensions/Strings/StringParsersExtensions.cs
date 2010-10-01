using System;

namespace Chucksoft.Core.Extensions.Strings
{
    public static class StringParsersExtensions
    {
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
    }
}