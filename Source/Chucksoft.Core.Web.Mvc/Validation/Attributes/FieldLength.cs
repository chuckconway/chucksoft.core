
namespace Chucksoft.Core.Web.Mvc.Validation.Attributes
{
    /// <summary>
    /// This validator will perform min length or max length tests
    /// </summary>
    public class FieldLength : FieldValidationBase
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public FieldLength(string errorMessage, string friendlyName, int minLength, int maxLength):base(errorMessage, friendlyName)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        public FieldLength(string errorMessage, string friendlyName, int maxLength) : this(errorMessage, friendlyName, 0, maxLength) { }

        /// <summary>
        /// Evalutes the specified val.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public override bool IsValid(string val)
        {
            return val.Length >= _minLength && val.Length <= _maxLength;
        }

        /// <summary>
        /// Gets the client implementation.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <value>The client implementation.</value>
        public override string ClientImplementation(string name)
        {
            return string.Empty;
        }
    }
}