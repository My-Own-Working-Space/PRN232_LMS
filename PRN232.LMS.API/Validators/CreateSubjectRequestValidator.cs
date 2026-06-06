using FluentValidation;
using PRN232.LMS.API.Models.Requests;
using System.Text.RegularExpressions;

namespace PRN232.LMS.API.Validators
{
    public class CreateSubjectRequestValidator : AbstractValidator<CreateSubjectRequest>
    {
        public CreateSubjectRequestValidator()
        {
            RuleFor(x => x.SubjectCode)
                .NotEmpty().WithMessage("SubjectCode is required")
                .Must(BeValidFptuSubjectCode).WithMessage("SubjectCode must follow FPTU style (e.g., PRN232, PRF192: 3 uppercase letters followed by 3 digits)");

            RuleFor(x => x.SubjectName)
                .NotEmpty().WithMessage("SubjectName is required")
                .MaximumLength(100).WithMessage("SubjectName cannot exceed 100 characters");

            RuleFor(x => x.Credit)
                .InclusiveBetween(1, 10).WithMessage("Credit must be between 1 and 10");
        }

        private bool BeValidFptuSubjectCode(string subjectCode)
        {
            if (string.IsNullOrEmpty(subjectCode)) return false;
            var regex = new Regex(@"^[A-Z]{3}\d{3}$");
            return regex.IsMatch(subjectCode);
        }
    }
}
