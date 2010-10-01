namespace Chucksoft.Core.Services
{
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the value by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetValueByKey(string key);
    }
}