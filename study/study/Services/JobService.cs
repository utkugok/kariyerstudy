using study.DTOs;
using study.Repositories;
using study.Repositories.Interfaces;
using study.Services.Interfaces;
using System.Net;

namespace study.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ICompanyRepository _companyRepository;

        public JobService(JobRepository jobRepository, CompanyRepository companyRepository)
        {
            _jobRepository = jobRepository;
            _companyRepository = companyRepository;
        }

        public async Task<ResponseDto<JobDto>> SaveAsync(JobCreateDto request)
        {
            try
            {
                var company = await _companyRepository.GetByIDAsync(request.companyId);

                if (company?.JobPostLimit is 0)
                {
                    throw new Exception("İlan yayınlama hakkınız yetersizdir.");
                }

                var responseJob = await _jobRepository.SaveAsync(request.CreateJob());

                var updateCompany = await _companyRepository.UpdateCompanyJobPostLimitAsync(request.companyId, company);

                if(!updateCompany)
                {
                    throw new Exception("Update company job posting limit error");
                }

                return ResponseDto<JobDto>.Success(responseJob.CreateDto(), HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ResponseDto<JobDto>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private async void UpdateCompanyJobPostingLimit(string companyId)
        {
        }
    }
}
