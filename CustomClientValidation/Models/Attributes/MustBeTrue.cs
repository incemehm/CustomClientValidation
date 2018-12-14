using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CustomClientValidation.Models.Attributes
{
    public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} has to be true";

        public MustBeTrueAttribute(string errorMessage = null) : base(errorMessage ?? DefaultErrorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            return value is bool && Convert.ToBoolean(value);
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var mustBeTrueRule = new ModelClientValidationRule();
            mustBeTrueRule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            mustBeTrueRule.ValidationType = "mustbechecked";

            yield return mustBeTrueRule;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }

    }
}