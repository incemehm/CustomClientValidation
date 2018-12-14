using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CustomClientValidation.Models.Attributes
{
    public class ValidateAgeAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} is invalid, it must be between {1} and {2}";

        private readonly DateTime _minimumDateProperty;
        private readonly DateTime _maximumDateProperty;

        public ValidateAgeAttribute(int minimumAgeProperty,int maximumAgeProperty, string errorMessage = null) : base(errorMessage ?? DefaultErrorMessage)
        {
            _minimumDateProperty = DateTime.Now.AddYears(minimumAgeProperty * -1);
            _maximumDateProperty = DateTime.Now.AddYears(maximumAgeProperty * -1);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime parsedValue = (DateTime)value;

                if (parsedValue <= _minimumDateProperty || parsedValue >= _maximumDateProperty)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;

        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "validateage",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
            };

            rule.ValidationParameters.Add("minumumdate", _minimumDateProperty.ToShortDateString());
            rule.ValidationParameters.Add("maximumdate", _maximumDateProperty.ToShortDateString());

            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, _minimumDateProperty.ToShortDateString(), _maximumDateProperty.ToShortDateString());
        }
    }
}