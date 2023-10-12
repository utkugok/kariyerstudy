using study.Models;

namespace study.DTOs
{
    public record CompanyCreateDto(string? phoneNumber, string? companyName, string? address)
    {
        public Company CreateCompany()
        {
            return new Company()
            {
                PhoneNumber = phoneNumber!,
                Address = address!,
                CompanyName = companyName!,
                JobPostLimit = 2
            };
        }
    }
}
