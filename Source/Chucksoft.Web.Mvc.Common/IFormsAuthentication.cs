namespace Chucksoft.Web.Mvc.Common
{
    public interface IFormsAuthentication
    {
        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        void SignIn(string userName, bool createPersistentCookie);

        /// <summary>
        /// Signs the out.
        /// </summary>
        void SignOut();
    }
}
