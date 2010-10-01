using System;

namespace Chucksoft.Core.Extensions
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// Determines if a number is even or not
        /// </summary>      
        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }
        /// <summary>
        /// Determines if a number is odd or not
        /// </summary>
        public static bool IsOdd(this int number)
        {
            return !number.IsEven();
        }
        /// <summary>
        /// if the number is a multiple of all supplied factors
        /// </summary>
        public static bool MultipleOf(this int number, params int[] factors)
        {
            return factors.Length != 0 && factors.TrueForAll(factor => number % factor == 0);
        }
        /// <summary>
        /// if the number is a factor of all supplied multiples
        /// </summary>
        public static bool FactorOf(this int number, params int[] multiples)
        {
            return multiples.Length != 0 && multiples.TrueForAll(multiple => multiple % number == 0);
        }    

        /// <summary>
        /// Returns the suffic (st, nd, rd, th) for the specified number
        /// </summary>      
        public static string Suffix(this int number)
        {
            if (number >= 10 && number < 20)
            {
                return "th";
            }
            switch (number % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
        /// <summary>
        /// Returns the suffix for the specified number appended to the number
        /// (1st, 12th, 33rd, 2nd)
        /// </summary>      
        public static string Suffixed(this int number)
        {
            return number + number.Suffix();
        }
        /// <summary>
        /// Performs the specified action a given number of times
        /// 3.times(i => sum += i);
        /// </summary>      
        public static void Times(this int times, Action<int> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            for (int i = 1; i <= times; ++i)
            {
                action(i);
            }
        }
        /// <summary>
        /// Performs the speficied action from start to end
        /// 3.UpTo(5, i => sum += i)
        /// </summary>      
        public static void UpTo(this int start, int end, Action<int> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            for (int i = start; i <= end; ++i)
            {
                action(i);
            }
        }
    }
}
