using System.Collections.Generic;


namespace Chucksoft.Core.Web.Mvc.Validation
{
    public interface IFormValidation
    {
        /// <summary>
        /// Validates the specified form.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        List<FieldValidationSummary> Validate<T>(T form);
    }
}