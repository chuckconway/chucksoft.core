using System.Web;
using Chucksoft.Core.Services;

namespace Chucksoft.Core.Web.Mvc.State
{
    public class SiteCookie : IMvcStateBag
    {
        private readonly HttpContext _context;
        private const string _cookieName = "SiteCookie";

        /// <summary>
        /// Gets the name of the cookie.
        /// </summary>
        /// <value>The name of the cookie.</value>
        public static string CookieName
        {
            get
            {
                return _cookieName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteCookie"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SiteCookie(HttpContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the value by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetValueByKey(string key)
        {
            string val = null;
            HttpCookie cookie = _context.Items[_cookieName] as HttpCookie;

            if (cookie != null)
            {
                val = cookie[key];
            }

            return val;
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string key, string value)
        {
            HttpCookie cookie = _context.Items[_cookieName] as HttpCookie;

            if (cookie != null)
            {
                cookie.Values[key] = value;
                _context.Items[_cookieName] = cookie;
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            HttpCookie cookie = _context.Items[_cookieName] as HttpCookie;

            if (cookie != null)
            {
                cookie.Values.Remove(key);
                _context.Items[_cookieName] = cookie;
            }
        }


        /// <summary>
        /// Called when [authorize request].
        /// </summary>
        public static void CookieDecryptionOnAuthorizeRequest(object sender, System.EventArgs e)
        {
            ICryptographyService service = new CryptographyService();

            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            HttpCookie calEmaCookie = new HttpCookie(CookieName);

            if (cookie != null)
            {
                calEmaCookie.Value = service.Decrypt(cookie.Value);
            }

            HttpContext.Current.Items[CookieName] = calEmaCookie;
        }
    }
}