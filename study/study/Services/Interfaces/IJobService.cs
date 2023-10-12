using study.DTOs;
using study.Models;

namespace study.Services.Interfaces
{
    public interface IJobService
    {
        Task<ResponseDto<JobDto>> SaveAsync(JobCreateDto request);

        Task<ResponseDto<List<JobDto>>> GetAllAsync();

        Task<ResponseDto<JobDto>> GetByIdAsync(string jobId);

        Task<ResponseDto<bool>> DeleteByIdAsync(string jobId);
    }
}
