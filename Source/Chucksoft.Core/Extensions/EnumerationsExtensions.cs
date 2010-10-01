
using System;
using System.Collections.Generic;

namespace Chucksoft.Core.Extensions
{
    public static class EnumerationsExtensions
    {      
        /// <summary>
        /// Performs the specified action on each element of the list
        /// </summary>
        public static void Each<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }

        /// <summary>
        /// Performs the specified action on each element of the list and includes
        /// an index value (starting at 0)
        /// </summary>
        public static void EachIndex<T>(this IEnumerable<T> list, Action<T, int> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            var enumerator = list.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
            {
                action(enumerator.Current, count++);
            }
        }

        /// <summary>
        /// Validates that the predicate is true for each element of the list
        /// </summary>      
        public static bool TrueForAll<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!predicate(enumerator.Current))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Retuns a list of all items matching the predicate
        /// </summary>      
        public static List<T> FindAll<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            var found = new List<T>();
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    found.Add(enumerator.Current);
                }
            }
            return found;
        }

        /// <summary>
        /// Retuns the first matching item
        /// </summary>      
        public static T Find<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {         
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }         
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    return enumerator.Current;
                }
            }
            return default(T);
        }

        /// <summary>
        /// Finds the index of an item
        /// </summary>      
        public static int Index<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            var enumerator = list.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); ++i)
            {
                if (predicate(enumerator.Current))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Determines whether or not the item exists
        /// </summary>      
        public static bool Exists<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            return list.Index(predicate) > -1;
        }
        
        /// <summary>
        /// Get a collection of the enumaration values.
        /// </summary>
        /// <returns></returns>
        public static IList<T> GetValues<T>()
        {
            IList<T> list = new List<T>();

            foreach (object value in Enum.GetValues(typeof(T)))
            {
                list.Add((T)value);
            }

            return list;
        }

        /// <summary>
        /// Get the enum value of the string passed in
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Gets the flag values.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static string[] GetFlagValues<T>(int flags)
        {
            List<string> list = new List<string>();
            foreach (object value in Enum.GetValues(typeof(T)))
            {
                int intValue = Convert.ToInt32((T)value);
                int intFlags = flags & intValue;
                if (intFlags == intValue)
                {
                    list.Add(Convert.ToString((T)value));
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Gets the string values.
        /// </summary>
        /// <returns></returns>
        public static string[] GetStringValues<T>()
        {
            List<string> list = new List<string>();

            foreach (object value in Enum.GetValues(typeof(T)))
            {
                list.Add(Convert.ToString((T)value));
            }

            return list.ToArray();
        }
    }
}