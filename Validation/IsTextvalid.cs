using System.ComponentModel.DataAnnotations;

namespace MyPetStore.Validation
{
    public class IsTextValid : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("This field cannot be empty");
            }
            if (value is string text)
            {
                text = text.Trim();

                if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text))
                {
                    return new ValidationResult("The field cannot contain only space ");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid data type.");

        }
    }
}
