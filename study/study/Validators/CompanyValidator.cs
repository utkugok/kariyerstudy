using FluentValidation;
using study.DTOs;

namespace study.Validators
{
    public class CompanyValidator : AbstractValidator<CompanyCreateDto>
    {
        public CompanyValidator()
        {
            RuleFor(r => r.phoneNumber)
                .NotEmpty().WithMessage("Phone number cannot be null or empty.")
                .MinimumLength(10).WithMessage("Phone number must be less than 10 characters.");

            RuleFor(r => r.companyName)
                .NotEmpty().WithMessage("Company Name cannot be null or empty.");

            RuleFor(r => r.address)
                .NotEmpty().WithMessage("Address cannot be null or empty.");
        }
    }
}
