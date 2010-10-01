using System;
using System.Web.Caching;

namespace Chucksoft.Core.Web.CacheProviders
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        void Add(string key, object val, TimeSpan slidingExpiration);

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="callback">The callback.</param>
        void Add(string key, object val, TimeSpan slidingExpiration, CacheItemUpdateCallback callback);

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="callback">The callback.</param>
        void Add(string key, object val, DateTime absoluteExpiration, CacheItemUpdateCallback callback);

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        void Add(string key, object val, DateTime absoluteExpiration);

        /// <summary>
        /// Retrieves the specified key name.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        object Retrieve(string keyName);

        /// <summary>
        /// Sets the specified key name.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="val">The val.</param>
        void Set(string keyName, object val);

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);
    }
}