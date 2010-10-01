namespace Chucksoft.Core.Web.Mvc.Validation.Attributes
{
    public interface IValidateAttribute
    {
        /// <summary>
        /// Evalutes the specified val.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        bool IsValid(string val);

        /// <summary>
        /// The Error message.
        /// </summary>
        /// <returns></returns>
        string ErrorMessage { get; }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        string FriendlyName { get;}

        /// <summary>
        /// Gets the client implementation.
        /// </summary>
        /// <value>The client implementation.</value>
        string ClientImplementation(string name);
    }
}