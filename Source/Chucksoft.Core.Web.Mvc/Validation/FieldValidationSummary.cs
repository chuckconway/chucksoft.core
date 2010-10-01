using System.Collections.Generic;

namespace Chucksoft.Core.Web.Mvc.Validation
{
    public class FieldValidationSummary
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValidationSummary"/> class.
        /// </summary>
        public FieldValidationSummary()
        {
            Failed = new List<FailedValidation>();
        }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the failed.
        /// </summary>
        /// <value>The failed.</value>
        public List<FailedValidation> Failed { get; set; }
    }
}