using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CustomClientValidation.Models.Attributes
{
    public class EqualsToPropertyAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} must be equal to {1}";
        private readonly string _toProperty;

        public EqualsToPropertyAttribute(string toProperty, string errorMessage = null): base(errorMessage ?? DefaultErrorMessage)
        {
            this._toProperty = toProperty;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var toPropertyInfo = validationContext.ObjectType.GetProperty(this._toProperty);
                if (toPropertyInfo.PropertyType.Equals(typeof(string)))
                {
                    string toBeValidated = (string)value;
                    string toBeReferenced = (string)toPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                    if (toBeValidated != toBeReferenced)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }
                else
                {
                    return new ValidationResult($"{_toProperty} is not of type string");
                }
            }   

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var equalStringToRule = new ModelClientValidationRule();
            equalStringToRule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            equalStringToRule.ValidationType = "equalstoproperty"; 
            equalStringToRule.ValidationParameters.Add("toproperty", _toProperty);

            yield return equalStringToRule;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, _toProperty);
        }

    }
}