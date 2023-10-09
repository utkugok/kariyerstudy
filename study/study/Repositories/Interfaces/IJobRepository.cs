using study.Models;

namespace study.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<Job> SaveAsync(Job newJob);
    }
}
