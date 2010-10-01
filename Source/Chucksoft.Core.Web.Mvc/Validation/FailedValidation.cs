namespace Chucksoft.Core.Web.Mvc.Validation
{
    public class FailedValidation
    {

        /// <summary>
        /// Gets or sets the user error message.
        /// </summary>
        /// <value>The user error message.</value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string InputValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName { get; set; }
    }
}