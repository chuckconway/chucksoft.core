using System;
using System.Security.Cryptography;

namespace Chucksoft.Core.Cryptography
{
    public static class Randomizer
    {
        private static readonly RNGCryptoServiceProvider _rand = new RNGCryptoServiceProvider();
        private static readonly byte[] _randb = new byte[4];

        /// <summary>
        /// Generates a random non-negative number.  
        /// </summary>
        public static int NextNumber()
        {
            _rand.GetBytes(_randb);
            int value = BitConverter.ToInt32(_randb, 0);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// Generates a random non-negative number less than or equal to max.
        /// </summary>
        /// <param name="max">The maximum possible value.</param>
        public static int NextNumber(int max)
        {
            _rand.GetBytes(_randb);
            int value = BitConverter.ToInt32(_randb, 0);

            value = value%(max + 1); // % calculates remainder
            if (value < 0)
            {
                value = -value;
            }
            return value;
        }

        /// <summary>
        /// Generates a random non-negative number bigger than or equal to min and less than or
        ///  equal to max.
        /// </summary>
        /// <param name="min">The minimum possible value.</param>
        /// <param name="max">The maximum possible value.</param>
        public static int NextNumber(int min, int max)
        {
            int value = NextNumber(max - min) + min;
            return value;
        }

        /// <summary>
        /// Randoms the alpha numeric characters.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomAlphaNumericCharacters(int length)
        {
            const string characters = "abcdefghijklmnpqrstuvwxyz123456789";
            return RandomAlphaNumericCharacters(length, characters);
        }

        /// <summary>
        /// Randoms the alpha numeric characters.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomAlphaCharacters(int length)
        {
            const string characters = "abcdefghijklmnpqrstuvwxyz";
            return RandomAlphaNumericCharacters(length, characters);
        }


        /// <summary>
        /// Randoms the captcha safe character.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string RandomCaptchaSafeCharacter(int length)
        {
            const string characters = "abcdefghkmnprstvwxyz2345689";
            return RandomAlphaNumericCharacters(length, characters);
        }


        /// <summary>
        /// Generate random AlphaNumericCharacters.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        private static string RandomAlphaNumericCharacters(int length, string characters)
        {
            string randomChars = string.Empty;
            char[] chars = characters.ToCharArray();

            for (int index = 0; index < length; index++)
            {
                int randomNumber = NextNumber(0, chars.Length - 1);
                randomChars += chars[randomNumber].ToString();
            }

            return randomChars;
        }
    }
}