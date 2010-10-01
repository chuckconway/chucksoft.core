namespace Chucksoft.Core.Web.Mvc.State
{
    public interface IMvcStateBag
    {
        /// <summary>
        /// Gets the value by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetValueByKey(string key);

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Set(string key, string value);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);
    }
}