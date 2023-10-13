using study.DTOs;
using study.Repositories;
using study.Repositories.Interfaces;
using study.Services.Interfaces;
using System.Net;
using System.Linq;
using FluentValidation;

namespace study.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<CompanyCreateDto> _validator;

        public CompanyService(ICompanyRepository companyRepository, IValidator<CompanyCreateDto> validator)
        {
            _companyRepository = companyRepository;
            _validator = validator;
        }

        public async Task<ResponseDto<CompanyDto>> SaveAsync(CompanyCreateDto request)
        {
            try
            {
                var result = await _validator.ValidateAsync(request!);
                if (!result.IsValid)
                {
                    return ResponseDto<CompanyDto>.Fail(result.Errors, HttpStatusCode.BadRequest);
                }

                var company = request.CreateCompany();

                var checkCompany = await _companyRepository.SearchByPhoneNumberAsync(company.PhoneNumber);

                if (checkCompany is not null)
                {
                    throw new Exception($"{request.phoneNumber} phone number already exists.");
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
            try
            {
                var companies = await _companyRepository.GetAllAsync();

                if( companies is null)
                {
                    return ResponseDto<List<CompanyDto>>.Success(null, HttpStatusCode.OK);
                }

                var companyListDto = companies.Select(x => new CompanyDto(x.Id, x.PhoneNumber, x.CompanyName, x.Address, x.JobPostLimit, x.CreatedAt, null)).ToList();

                return ResponseDto<List<CompanyDto>>.Success(companyListDto, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<CompanyDto>>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto<CompanyDto>> GetByIdAsync(string companyId)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(companyId);

                if (company is null)
                {
                    throw new Exception($"Company not found. Check your company id : {companyId}");
                }

                return ResponseDto<CompanyDto>.Success(company.CreateDto(), HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ResponseDto<CompanyDto>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
