using study.Models;
using System.ComponentModel.DataAnnotations;

namespace study.DTOs
{
    public record CompanyDto(string id, string phoneNumber, string companyName, string address, int jobPostLimit, DateTime createdAt, List<JobDto>? job)

    {
    }
}
