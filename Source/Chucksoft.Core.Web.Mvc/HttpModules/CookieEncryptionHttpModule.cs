using System;
using System.Web;
using Chucksoft.Core.Services;
using Chucksoft.Core.Web.Mvc.State;

namespace Chucksoft.Core.Web.Mvc.HttpModules
{
    public class CookieEncryptionHttpModule : IHttpModule
    {
        private readonly ICryptographyService _cryptographyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieEncryptionHttpModule"/> class.
        /// </summary>
        /// <param name="cryptographyService">The cryptography service.</param>
        public CookieEncryptionHttpModule(ICryptographyService cryptographyService)
        {
            _cryptographyService = cryptographyService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieEncryptionHttpModule"/> class.
        /// </summary>
        public CookieEncryptionHttpModule() : this(new CryptographyService()) { }

        private HttpApplication _context;

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            _context = context;
            context.PreSendRequestHeaders += ContextPreSendRequestHeaders;
        }

        /// <summary>
        /// Handles the PreSendRequestHeaders event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void ContextPreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpCookie cookie = _context.Context.Items[SiteCookie.CookieName] as HttpCookie;

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                HttpCookie calEmaCookie = new HttpCookie(SiteCookie.CookieName) { Value = _cryptographyService.Encrypt(cookie.Value) };

                _context.Response.Cookies.Remove(SiteCookie.CookieName);
                _context.Response.Cookies.Add(calEmaCookie);
            }
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose(){ }
    }
}