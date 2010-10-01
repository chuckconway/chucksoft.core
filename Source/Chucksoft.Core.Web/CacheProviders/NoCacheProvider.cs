using System;
using System.Web.Caching;

namespace Chucksoft.Core.Web.CacheProviders
{
    public class NoCacheProvider : ICacheProvider
    {
        #region ICacheProvider Members

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        public void Add(string key, object val, TimeSpan slidingExpiration)
        {
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="callback">The callback.</param>
        public void Add(string key, object val, TimeSpan slidingExpiration, CacheItemUpdateCallback callback)
        {

        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="callback">The callback.</param>
        public void Add(string key, object val, DateTime absoluteExpiration, CacheItemUpdateCallback callback)
        {

        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        public void Add(string key, object val, DateTime absoluteExpiration)
        {
        }

        /// <summary>
        /// Retrieves the specified key name.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public object Retrieve(string keyName)
        {
            return null;
        }

        /// <summary>
        /// Sets the specified key name.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="val">The val.</param>
        public void Set(string keyName, object val)
        {
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
        }

        #endregion
    }
}