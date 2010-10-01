using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Chucksoft.Core.Web.Mvc.Validation.Attributes;

namespace Chucksoft.Core.Web.Mvc.Validation
{
    public class Validator<T>
    {
        private readonly T _form;

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        /// <value>The validation results.</value>
        public List<FieldValidationSummary> ValidationResults { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Validator&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="form">The form.</param>
        public Validator(T form){_form = form;}


        /// <summary>
        /// Validates the specified form.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            ValidationResults = new List<FieldValidationSummary>();
            PropertyInfo[] properties = _form.GetType().GetProperties();

            for (int index = 0; index < properties.Length; index++)
            {
                FieldValidationSummary result = EvaluteUserInputForField(properties[index], _form);

                if (result != null && result.Failed.Count > 0)
                {
                    ValidationResults.Add(result);
                }
            }

           return ValidationResults.Count == 0;
        }

        /// <summary>
        /// Evalutes the user input for field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        private static FieldValidationSummary EvaluteUserInputForField(PropertyInfo property, T form)
        {
            FieldValidationSummary result = null;
            List<IValidateAttribute> fieldCritera = GetValidationCriteraForProperty(property);

            if(fieldCritera.Count > 0)
            {
                object val = property.GetValue(form, null);
                string propertyValue = Convert.ToString(val);
                result = ValidateField(property, fieldCritera, propertyValue);
            }

            return result;
        }

        /// <summary>
        /// Validates the field.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="fieldCritera">The field critera.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <returns></returns>
        private static FieldValidationSummary ValidateField(PropertyInfo property, IList<IValidateAttribute> fieldCritera, string propertyValue)
        {
            FieldValidationSummary result = new FieldValidationSummary { FieldName = property.Name };

            for (int indx = 0; indx < fieldCritera.Count; indx++)
            {
                if(!fieldCritera[indx].IsValid(propertyValue))
                {
                    FailedValidation validation = new FailedValidation { ErrorMessage = fieldCritera[indx].ErrorMessage, InputValue = propertyValue, FriendlyName = (string.IsNullOrEmpty(fieldCritera[indx].FriendlyName)? property.Name : fieldCritera[indx].FriendlyName)};
                    result.Failed.Add(validation);
                }
            }

            return result;
        }


        /// <summary>
        /// Gets the validation critera for property.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        private static List<IValidateAttribute> GetValidationCriteraForProperty(ICustomAttributeProvider propertyInfo)
        {
            object[] attributes = propertyInfo.GetCustomAttributes(false);
            List<IValidateAttribute> fieldValidation = new List<IValidateAttribute>();

            for (int idx = 0; idx < attributes.Length; idx++)
            {
                Type type = attributes[idx].GetType().GetInterface("IValidateAttribute");

                if (type != null)
                {
                    IValidateAttribute validate = (IValidateAttribute)attributes[idx];
                    fieldValidation.Add(validate);
                }
            }

            return fieldValidation;
        }

        /// <summary>
        /// Generates the client side validation.
        /// </summary>
        /// <returns></returns>
        public string GenerateClientSideValidation()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<script type=\"text/javascript\">");
            bool hasValidators = GetValidators(builder);
            builder.AppendLine("</script> ");

            return (hasValidators ? builder.ToString() : string.Empty);
        }

        /// <summary>
        /// Ges the validators.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        private bool GetValidators(StringBuilder builder)
        {
            PropertyInfo[] properties = _form.GetType().GetProperties();
            bool hasValidators = false;

            for (int index = 0; index < properties.Length; index++)
            {
                List<IValidateAttribute> fieldCritera = GetValidationCriteraForProperty(properties[index]);

                if(fieldCritera.Count > 0)
                {
                    builder.AppendLine("//Validation for " + properties[index].Name);

                    hasValidators = true;
                    int index0 = index;
                    fieldCritera.ForEach(o => builder.AppendLine(o.ClientImplementation(properties[index0].Name)));
                }

                builder.AppendLine(" ");
            }

            return hasValidators;
        }
    }
}