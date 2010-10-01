using System.Web;
using System.Web.Security;
using Chucksoft.Core;

namespace Chucksoft.Web.Mvc.Common.Authentication 
{
    public class UserSessionService<T, TId> : IUserSession<T> where T : IId<TId>
    {
        #region IUserSession Members

        /// <summary>
        /// Logins the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        /// <param name="redirectPath">The redirect path.</param>
        public void Login(T user, bool persistent, string redirectPath) 
        {
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), persistent);
            HttpContext.Current.Items["User"] = user;
            HttpContext.Current.Response.Redirect(redirectPath);
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        public void LogUserOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Response.Redirect("~/");
        }

        #endregion
    }
}
