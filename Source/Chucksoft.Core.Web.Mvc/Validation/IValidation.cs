using System.Collections.Generic;

namespace Chucksoft.Core.Web.Mvc.Validation
{
    public interface IValidation
    {
        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        List<FieldValidationSummary> ValidationErrors();

        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        bool IsValid { get;}
    }
}