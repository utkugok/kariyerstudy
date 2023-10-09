using study.DTOs;
using study.Models;
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
        private readonly IProhibitedWordsService _prohibitedWordsService;

        public JobService(IJobRepository jobRepository, ICompanyRepository companyRepository, IProhibitedWordsService prohibitedWordsService)
        {
            _jobRepository = jobRepository;
            _companyRepository = companyRepository;
            _prohibitedWordsService = prohibitedWordsService;
        }

        public async Task<ResponseDto<JobDto>> SaveAsync(JobCreateDto request)
        {
            try
            {
                var company = await _companyRepository.GetByIDAsync(request.companyId);

                if (company  is null)
                {
                    throw new Exception("Check your company id");
                }

                if (company?.JobPostLimit is 0)
                {
                    throw new Exception("İlan yayınlama hakkınız yetersizdir.");
                }
                var job = request.CreateJob();
                job.Quality = QualityScore(job.WorkType, job.Salary, job.Benefits, job.Description);

                var responseJob = await _jobRepository.SaveAsync(job);

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

        private int QualityScore(string? workType, decimal? salary, string? benefits, string description)
        {
            int qualityScore = 0;
            if (workType is not null)
            {
                qualityScore += 1;
            }
            if (salary is not null)
            {
                qualityScore += 1;
            }
            if (benefits is not null)
            {
                qualityScore += 1;
            }
            if (_prohibitedWordsService.CheckProhibitedWordInDescription(description) == false)
            {
                qualityScore += 2;
            }

            return qualityScore;
        }
    }
}
