using study.Models;
using study.Services;
using study.Services.Interfaces;

namespace study.DTOs
{
    public record JobCreateDto(string companyId, string position, string description, decimal? salary, string? benefits, string? workType)
    {
        public Job CreateJob()
        {
            return new Job()
            {
                CompanyId = companyId,
                Position = position,
                Description = description,
                DurationInDays = 14,
                Salary = salary,
                WorkType = workType,
                Benefits = benefits,
                Status = 1
            };
        }

      
    }
}
