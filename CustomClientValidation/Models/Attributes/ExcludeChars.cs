using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CustomClientValidation.Models.Attributes
{
    public class ExcludeCharsAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _chars;
        private const string DefaultErrorMessage = "{0} has invalid characters";

        public ExcludeCharsAttribute(string chars, string errorMessage = null) : base(errorMessage ?? DefaultErrorMessage)
        {
            _chars = chars;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                for (int i = 0; i < _chars.Length; i++)
                {
                    if (valueAsString.Contains(_chars[i].ToString()))
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }
            }
            return ValidationResult.Success;
        }
       
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "excludechars";
            rule.ValidationParameters.Add("chars", _chars);
            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
    }
}