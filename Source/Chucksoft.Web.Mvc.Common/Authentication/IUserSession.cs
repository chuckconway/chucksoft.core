namespace Chucksoft.Web.Mvc.Common.Authentication
{
    public interface IUserSession<T>
    {
        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        /// <param name="redirectPath">The redirect path.</param>
        void Login(T user, bool persistent, string redirectPath);


        /// <summary>
        /// Logs the user out.
        /// </summary>
        void LogUserOut();
    }
}
