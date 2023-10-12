using study.Models;

namespace study.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company> SaveAsync(Company newCompany);

        Task<Company?> SearchByPhoneNumberAsync(string phoneNumber);

        Task<bool> UpdateCompanyJobPostLimitAsync(string companyId, Company company);

        Task<IReadOnlyCollection<Company>> GetAllAsync();

        Task<Company?> GetByIdAsync(string companyId);
    }
}