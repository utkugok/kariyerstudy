using Elastic.Clients.Elasticsearch.Fluent;
using Elastic.Clients.Elasticsearch.Requests;
using FluentValidation;
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
        private readonly IValidator<JobCreateDto> _validator;

        public JobService(IJobRepository jobRepository, ICompanyRepository companyRepository, IProhibitedWordsService prohibitedWordsService, IValidator<JobCreateDto> validator)
        {
            _jobRepository = jobRepository;
            _companyRepository = companyRepository;
            _prohibitedWordsService = prohibitedWordsService;
            _validator = validator;
        }

        public async Task<ResponseDto<JobDto>> SaveAsync(JobCreateDto request)
        {
            try
            {
                var result = await _validator.ValidateAsync(request!);
                if (!result.IsValid)
                {
                    return ResponseDto<JobDto>.Fail(result.Errors, HttpStatusCode.BadRequest);
                }

                var company = await _companyRepository.GetByIdAsync(request.companyId!);

                if (company is null)
                {
                    throw new Exception("Company not found. Check your company id");
                }

                if (company.JobPostLimit is 0)
                {
                    throw new Exception("Job post limit is insufficient.");
                }

                var job = request.CreateJob();

                job.Quality = QualityScore(job.WorkType, job.Salary, job.Benefits, job.Description);

                var prohibitedWordInDescription = await _prohibitedWordsService.CheckProhibitedWordInDescriptionAsync(job.Description);

                if (prohibitedWordInDescription == false)
                {
                    job.Quality += 2;
                }

                var responseJob = await _jobRepository.SaveAsync(job);

                var updateCompany = await _companyRepository.UpdateCompanyJobPostLimitAsync(request.companyId!, company);

                if (!updateCompany)
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

        public async Task<ResponseDto<List<JobDto>>> GetAllAsync()
        {
            try
            {
                var jobs = await _jobRepository.GetAllAsync();

                if(jobs is null )
                {
                    return ResponseDto<List<JobDto>>.Success(null, HttpStatusCode.OK);
                }

                var jobListDto = jobs.Select(x => new JobDto(x.Id, x.CompanyId, x.Position, x.Description, x.DurationInDays, x.Quality, x.Benefits, x.WorkType, x.Salary, x.CreatedAt)).ToList();

                return ResponseDto<List<JobDto>>.Success(jobListDto, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<JobDto>>.Fail(ex.Message, HttpStatusCode.InternalServerError);
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

            return qualityScore;
        }

        public async Task<ResponseDto<JobDto>> GetByIdAsync(string jobId)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(jobId);

                if (job is null)
                {
                    throw new Exception($"Job not found. Check your job id : {jobId}");
                }

                return ResponseDto<JobDto>.Success(job.CreateDto(), HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ResponseDto<JobDto>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(string jobId)
        {
            try
            {
                var isSuccess = await _jobRepository.DeleteByIdAsync(jobId);

                if (isSuccess is false)
                {
                    throw new Exception($"Job not found. Check your job id : {jobId}");
                }

                return ResponseDto<bool>.Success(isSuccess, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
