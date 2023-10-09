using study.DTOs;

namespace study.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<ResponseDto<CompanyDto>> SaveAsync(CompanyCreateDto request);
    }
}
