using FluentValidation;
using study.DTOs;

namespace study.Validators
{
    public class JobValidator : AbstractValidator<JobCreateDto>
    {
        public JobValidator()
        {
            RuleFor(r => r.position)
                .NotEmpty().WithMessage("Job Position cannot be null or empty.");

            RuleFor(r => r.description)
                .NotEmpty().WithMessage("Job Description cannot be null or empty.");

            RuleFor(r => r.companyId)
                .NotEmpty().WithMessage("Job CompanyID cannot be null or empty.");

            RuleFor(r => r.salary)
                .GreaterThan(0).WithMessage("Job Salary must be grater than 0(zero).");
        }
    }
}
