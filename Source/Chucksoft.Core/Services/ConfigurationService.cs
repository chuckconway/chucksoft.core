using System.Collections.Specialized;
using System.Configuration;

namespace Chucksoft.Core.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly NameValueCollection _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
        /// </summary>
        public ConfigurationService()
        {
            _appSettings = ConfigurationManager.AppSettings;
        }

        /// <summary>
        /// Gets the value by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetValueByKey(string key)
        {
            return _appSettings[key];
        }
    }
}