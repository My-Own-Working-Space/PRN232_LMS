using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PRN232.LMS.API.Models.Requests
{
    public class FptuCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            
            var code = value.ToString();
            if (string.IsNullOrEmpty(code)) return ValidationResult.Success;

            var regex = new Regex(@"^[A-Z]{2}\d{5}$");
            if (!regex.IsMatch(code))
            {
                return new ValidationResult(ErrorMessage ?? "Code must follow FPTU style (e.g., SE19886, CE18793: 2 uppercase letters followed by 5 digits)");
            }

            return ValidationResult.Success;
        }
    }
}
