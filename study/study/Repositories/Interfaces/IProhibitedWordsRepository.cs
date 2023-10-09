using study.DTOs;
using study.Models;

namespace study.Repositories.Interfaces
{
    public interface IProhibitedWordsRepository
    {

        Task<ProhibitedWord> SaveAsync(ProhibitedWord newProhibitedWord);

        Task<ProhibitedWord?> SearchByProhibitedWordAsync(string prohibitedWord);

        Task<IReadOnlyCollection<ProhibitedWord>> GetAllAsync();



    }
}
