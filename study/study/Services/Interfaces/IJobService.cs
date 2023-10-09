using study.DTOs;

namespace study.Services.Interfaces
{
    public interface IJobService
    {
        Task<ResponseDto<JobDto>> SaveAsync(JobCreateDto request);
    }
}
