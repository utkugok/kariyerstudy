using study.Models;

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
                Quality = QualityScore(workType, salary, benefits, description),
                Salary = salary,
                WorkType = workType,
                Benefits = benefits,
                Status = 1
            };
        }

        private int QualityScore(string? workType, decimal? salary, string? benefits, string description)
        {
            int qualityScore=0;
            if(workType is not null)
            {
                qualityScore += 1;
            }
            if (salary is not null)
            {
                qualityScore += 1;
            }
            if (benefits is not null)
            {
                qualityScore += 1;
            }
            if (!description.Contains("deli")) //TODO (Utku): cache ten kontrol et
            {
                qualityScore += 2;
            }

            return qualityScore;
        }
    }
}
