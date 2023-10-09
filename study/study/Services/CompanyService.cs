using study.DTOs;
using study.Repositories;
using study.Repositories.Interfaces;
using study.Services.Interfaces;
using System.Net;

namespace study.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;                
        }
        
        public async Task<ResponseDto<CompanyDto>> SaveAsync(CompanyCreateDto request)
        {
            try
            {
                var company = request.CreateCompany();

                var checkCompany = await _companyRepository.SearchByPhoneNumberAsync(company.PhoneNumber);

                if(checkCompany is not null)
                {
                    throw new Exception($"{request.phoneNumber} telefon numarası zaten kayıtlıdır.");
                }

                var responseCompany = await _companyRepository.SaveAsync(company);

                return ResponseDto<CompanyDto>.Success(responseCompany.CreateDto(), HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ResponseDto<CompanyDto>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
