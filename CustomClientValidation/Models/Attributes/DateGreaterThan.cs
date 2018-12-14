using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CustomClientValidation.Models.Attributes
{
    public class DateGreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} must be greater than {1}";
        private readonly string _toProperty;

        public DateGreaterThanAttribute(string toProperty, string errorMessage = null): base(errorMessage ?? DefaultErrorMessage)
        {
            this._toProperty = toProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(_toProperty);
                if (otherPropertyInfo.PropertyType.Equals(new DateTime().GetType()))
                {
                    DateTime toBeValidated = (DateTime)value;
                    DateTime toBeReferenced = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                    if (toBeValidated.CompareTo(toBeReferenced) < 1)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }
                else
                {
                    return new ValidationResult($"{_toProperty} is not of type DateTime");
                }
            }

            return ValidationResult.Success;
        }


        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var dateGreaterThanRule = new ModelClientValidationRule();
            dateGreaterThanRule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()); 
            dateGreaterThanRule.ValidationType = "dategreaterthan";
            dateGreaterThanRule.ValidationParameters.Add("toproperty", _toProperty);

            yield return dateGreaterThanRule;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, _toProperty);
        }

    }
}