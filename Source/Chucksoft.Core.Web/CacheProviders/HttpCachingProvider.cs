using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Chucksoft.Core.Web.CacheProviders
{
    public class HttpCachingProvider : ICacheProvider
    {
        #region ICacheProvider Members

        /// <summary>
        /// Add to Cache
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        public void Add(string key, object val, TimeSpan slidingExpiration)
        {
            HttpContext.Current.Cache.Insert(key, val, null, DateTime.MaxValue, slidingExpiration);
        }

        /// <summary>
        /// Add to Cache
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="callback">The callback.</param>
        public void Add(string key, object val, TimeSpan slidingExpiration, CacheItemUpdateCallback callback)
        {
            HttpContext.Current.Cache.Insert(key, val, null, DateTime.MaxValue, slidingExpiration);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The val.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        public void Add(string key, object val, DateTime absoluteExpiration)
        {
            HttpContext.Current.Cache.Insert(key, val, null, absoluteExpiration, TimeSpan.Zero);
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
            HttpContext.Current.Cache.Insert(key, val, null, absoluteExpiration, TimeSpan.Zero, callback);
        }

        /// <summary>
        /// Clears cache
        /// </summary>
        public void Clear()
        {
            foreach (DictionaryEntry item in HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Insert(item.Key.ToString(), null);
            }
        }

        /// <summary>
        /// Retrieve the object value by Key name
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public object Retrieve(string keyName)
        {
            try
            {
                return HttpContext.Current.Cache[keyName];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the specified key name.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="val">The val.</param>
        public void Set(string keyName, object val)
        {
            HttpContext.Current.Cache[keyName] = val;
        }

        /// <summary>
        /// Removes key from site cache
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        #endregion
    }
}