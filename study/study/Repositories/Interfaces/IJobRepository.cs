using study.Models;

namespace study.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<Job> SaveAsync(Job newJob);

        Task<IReadOnlyCollection<Job>> GetAllAsync();

        Task<Job?> GetByIdAsync(string jobId);

        Task<bool> DeleteByIdAsync(string jobId);            
    }
}
