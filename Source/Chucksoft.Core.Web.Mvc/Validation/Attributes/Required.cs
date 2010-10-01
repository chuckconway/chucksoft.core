namespace Chucksoft.Core.Web.Mvc.Validation.Attributes
{
    /// <summary>
    /// Field is required. It can not be null or empty.
    /// </summary>
    public class Required : FieldValidationBase
    {
        public override string ClientImplementation(string fieldName)
        {
            const string validation = "var {0}Validation = new LiveValidation('{0}',{{ validMessage: \" \", onlyOnBlur: true }});" +
                                      "{0}Validation.add( Validate.Presence, {{failureMessage: \"{1}\" }} );";

            return string.Format(validation, fieldName, string.Format(ErrorMessage, FriendlyName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Required"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public Required(string errorMessage): base(errorMessage) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Required"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="friendlyName">Name of the friendly.</param>
        public Required(string errorMessage, string friendlyName): base(errorMessage, friendlyName){}

        /// <summary>
        /// Evalutes the specified val.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public override bool IsValid(string val)
        {
            if(val != null)
            {
                val = val.Trim();
            }

            bool hasValue = !string.IsNullOrEmpty(val);
            return hasValue;
        }
    }
}