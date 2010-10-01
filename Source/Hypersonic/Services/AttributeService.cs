using System.Linq;
using System.Reflection;
using Hypersonic.Attributes;

namespace Hypersonic.Services
{
    internal class AttributeService
    {
        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        public string GetAlias(PropertyInfo propertyInfo)
        {
            string alias = string.Empty;
            object[] attributes = propertyInfo.GetCustomAttributes(typeof (DataAlias), true);

            foreach (DataAlias attribute in attributes.OfType<DataAlias>())
            {
                alias = attribute.Alias;
                break;
            }

            return alias;
        }

        /// <summary>
        /// Determines whether the specified property info has ignore.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>
        /// 	<c>true</c> if the specified property info has ignore; otherwise, <c>false</c>.
        /// </returns>
        public bool HasParameterIgnore(PropertyInfo propertyInfo)
        {
            return HasIgnore<IgnoreParameter>(propertyInfo);
        }

        /// <summary>
        /// Determines whether the specified property info has ignore.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>
        /// 	<c>true</c> if the specified property info has ignore; otherwise, <c>false</c>.
        /// </returns>
        public bool HasHydrationIgnore(PropertyInfo propertyInfo)
        {
            return HasIgnore<IgnoreHydration>(propertyInfo);
        }

        /// <summary>
        /// Determines whether the specified property info has ignore.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>
        /// 	<c>true</c> if the specified property info has ignore; otherwise, <c>false</c>.
        /// </returns>
        private static bool HasIgnore<T>(PropertyInfo propertyInfo)
        {
            object[] ignoreParameterAttributes = propertyInfo.GetCustomAttributes(typeof(T), true);
            object[] ignoreParameterAndHydrationAttributes = propertyInfo.GetCustomAttributes(typeof(IgnoreParameterAndHydration), true);
            bool hasIgnore = ignoreParameterAttributes.OfType<T>().Any() || ignoreParameterAndHydrationAttributes.OfType<IgnoreParameterAndHydration>().Any();
            return hasIgnore;
        }

        /// <summary>
        /// Sets the alias.
        /// </summary>
        /// <param name="propertyInfos">The property infos.</param>
        /// <returns></returns>
        public PropertyInfoDecorator[] SetAlias(PropertyInfo[] propertyInfos)
        {
            return (from propertyInfo in propertyInfos
                    let alias = GetAlias(propertyInfo)
                    select new PropertyInfoDecorator
                               {
                                   Alias = alias, Property = propertyInfo
                               }).ToArray();
        }
    }
}
