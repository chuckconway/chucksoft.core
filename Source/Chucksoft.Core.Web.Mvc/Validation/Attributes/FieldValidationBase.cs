using System;

namespace Chucksoft.Core.Web.Mvc.Validation.Attributes
{
    public abstract class FieldValidationBase : Attribute, IValidateAttribute
    {
        private readonly string _errorMessage;
        private readonly string _friendlyName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValidationBase"/> class.
        /// </summary>
        protected FieldValidationBase(){}

        /// <summary>
        /// The Error message.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string ErrorMessage { get { return _errorMessage; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Required"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        protected FieldValidationBase(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Required"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="friendlyName">Name of the friendly.</param>
        protected FieldValidationBase(string errorMessage, string friendlyName)
        {
            _errorMessage = errorMessage;
            _friendlyName = friendlyName;
        }

        /// <summary>
        /// Evalutes the specified val.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public abstract bool IsValid(string val);


        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName
        {
            get { return _friendlyName; }
        }

        /// <summary>
        /// Gets the client implementation.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <value>The client implementation.</value>
        public abstract string ClientImplementation(string name);

    }
}