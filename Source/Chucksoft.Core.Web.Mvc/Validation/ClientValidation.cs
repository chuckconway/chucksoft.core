using System.Web.Mvc;

namespace Chucksoft.Core.Web.Mvc.Validation
{
    public static class ClientValidationHelper
    {
        /// <summary>
        /// Clients the validation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        public static string ClientValidation<T>(this HtmlHelper helper, T form )
        {
            Validator<T> validator = new Validator<T>(form);
            return validator.GenerateClientSideValidation();
        }
    }
}