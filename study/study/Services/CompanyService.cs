using study.DTOs;
using study.Repositories;
using study.Repositories.Interfaces;
using study.Services.Interfaces;
using System.Net;
using System.Linq;

namespace study.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<ResponseDto<CompanyDto>> SaveAsync(CompanyCreateDto request)
        {
            try
            {
                var company = request.CreateCompany();

                var checkCompany = await _companyRepository.SearchByPhoneNumberAsync(company.PhoneNumber);

                if (checkCompany is not null)
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

        public async Task<ResponseDto<List<CompanyDto>>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();

            var companyListDto = companies.Select(x => new CompanyDto(x.Id, x.PhoneNumber, x.CompanyName, x.Address, x.JobPostLimit, x.CreatedAt, null)).ToList();

            return ResponseDto<List<CompanyDto>>.Success(companyListDto, HttpStatusCode.OK);
        }

    }
}
