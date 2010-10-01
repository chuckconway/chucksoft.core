using System;
using System.Collections.Generic;
using System.Reflection;

namespace Chucksoft.Core.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Determines if both account collections are equal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="otherCollection">The other collection.</param>
        /// <returns></returns>
        public static bool AreBothCollectionsEqual<T>(this IList<T> collection, IList<T> otherCollection) where T : IEquatable<T>
        {
            bool isEqual = true;
            if (collection.Count.Equals(otherCollection.Count))
            {
                for (int index = 0; index < collection.Count; index++)
                {
                    if (!collection[index].Equals(otherCollection[index]))
                    {
                        isEqual = false;
                        break;
                    }
                }
            }
            else
            {
                isEqual = false;
            }

            return isEqual;
        }

        /// <summary>
        /// Determines whether the specified source is equal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source is equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEqual<T>(this T source, T comparer) where T : class
        {
            bool isEqual = true;

            PropertyInfo[] sourceInfo = source.GetType().GetProperties();
            PropertyInfo[] comparerInfo = source.GetType().GetProperties();

            for (int index = 0; index < sourceInfo.Length; index++)
            {
                if (sourceInfo[index].CanRead && comparerInfo[index].CanRead)
                {
                    object sourceValue = sourceInfo[index].GetValue(source, null);
                    object comparerValue = comparerInfo[index].GetValue(comparer, null);

                    isEqual = (sourceValue == comparerValue);

                    if (!isEqual)
                    {
                        break;
                    }
                }
            }

            return isEqual;
        }
    }
}
