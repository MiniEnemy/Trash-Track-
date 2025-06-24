using System.ComponentModel.DataAnnotations;

namespace Trash_Track.Admin
{
    public class CompareDateAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;

        public CompareDateAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDate = (DateTime?)value;
            var property = validationContext.ObjectType.GetProperty(_otherProperty);
            var startDate = (DateTime?)property?.GetValue(validationContext.ObjectInstance);

            if (startDate != null && endDate != null && endDate < startDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success!;
        }
    }

}
