using System.Web;

namespace Chucksoft.Core.Web
{
    public static class HttpRequestExtensions
    {
        private static readonly string[] _ipHeaderOrder = new[] { "HTTP_X_FORWARDED_FOR", "HTTP_X_CLUSTER_CLIENT_IP", "REMOTE_ADDR" };

        /// <summary>
        /// Returns the client's real IP address, taking into accout some proxying and clusters
        /// </summary>
        /// <remarks>
        /// Many proxies don't expose the real client's IP address. In such cases, the proxies 
        /// IP address will be returned
        /// </remarks>
        /// <returns>The best guess at a client's IP address, or an empty string</returns>
        public static string ClientAddress(this HttpRequest request)
        {
            foreach (string header in _ipHeaderOrder)
            {
                string ipAddress = request.ServerVariables[header];
                if (ipAddress != null)
                {
                    return ipAddress;
                }
            }

            return string.Empty;
        }
    }
}