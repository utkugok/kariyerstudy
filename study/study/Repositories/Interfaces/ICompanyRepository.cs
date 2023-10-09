using study.Models;

namespace study.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company> SaveAsync(Company newCompany);

        Task<Company?> SearchByPhoneNumberAsync(string phoneNumber);

        Task<Company?> GetByIDAsync(string id);

        Task<bool> UpdateCompanyJobPostLimitAsync(string companyId, Company company);
    }
}
