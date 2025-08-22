using System.ComponentModel.DataAnnotations;

namespace Training_ITI.Models
{
    public class StartNotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dt && dt.Date < DateTime.Today)
                return new ValidationResult("Start date cannot be in the past.");
            return ValidationResult.Success;
        }
    }

    // EndDate > StartDate
    [AttributeUsage(AttributeTargets.Property)]
    public class EndAfterAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;
        public EndAfterAttribute(string otherProperty) => _otherProperty = otherProperty;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherProp = validationContext.ObjectType.GetProperty(_otherProperty);
            if (otherProp == null) return ValidationResult.Success;

            var otherVal = otherProp.GetValue(validationContext.ObjectInstance);
            if (value is DateTime end && otherVal is DateTime start)
            {
                if (end <= start)
                    return new ValidationResult("End date must be after start date.");
            }
            return ValidationResult.Success;
        }
    }
}