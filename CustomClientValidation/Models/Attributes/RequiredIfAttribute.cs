using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CustomClientValidation.Models.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "The {0} field is required.";

        private readonly string _toProperty;
        private readonly Comparison _comparison;
        private readonly object _comparisonValue;

        public RequiredIfAttribute(string toProperty, Comparison comparison, object value, string errorMessage = null) : base(errorMessage ?? DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(toProperty))
            {
                throw new ArgumentNullException("toProperty");
            }

            _toProperty = toProperty;
            _comparison = comparison;
            _comparisonValue = value;
        }

        public RequiredIfAttribute(string toProperty, Comparison comparison, string errorMessage = null) : 
            this(toProperty, comparison, null, errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var toPropertyInfo = validationContext.ObjectInstance.GetType().GetProperty(_toProperty);
            var toPropertyValue = toPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (IsRequired(toPropertyValue) && (value == null || string.IsNullOrEmpty(value?.ToString())))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var requiredIfRule = new ModelClientValidationRule();
            requiredIfRule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            requiredIfRule.ValidationType = "requiredif";
            requiredIfRule.ValidationParameters.Add("toproperty", _toProperty);
            requiredIfRule.ValidationParameters.Add("comparison", _comparison.ToString().ToLowerInvariant());
            requiredIfRule.ValidationParameters.Add("comparisonvalue", _comparisonValue?.ToString().ToLowerInvariant());

            yield return requiredIfRule;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
        
        private bool IsRequired(object toPropertyValue)
        {
            switch (_comparison)
            {
                case Comparison.IsEqualTo:
                    return toPropertyValue != null && toPropertyValue.Equals(_comparisonValue);
                case Comparison.IsNotEqualTo:
                    return toPropertyValue == null || !toPropertyValue.Equals(_comparisonValue);
                case Comparison.IsEmpty:
                    return toPropertyValue == null || string.IsNullOrEmpty(toPropertyValue?.ToString());
                case Comparison.IsNotEmpty:
                    return toPropertyValue != null && !string.IsNullOrEmpty(toPropertyValue?.ToString());
                default:
                    return toPropertyValue != null && toPropertyValue.Equals(_comparisonValue);
            }
        }

    }

    public enum Comparison
    {
        IsEqualTo,
        IsNotEqualTo,
        IsEmpty,
        IsNotEmpty
    }
}