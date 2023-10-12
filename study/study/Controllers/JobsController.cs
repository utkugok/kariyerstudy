using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using study.DTOs;
using study.Models;
using study.Services;
using study.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : BaseController
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "SaveAsync",
            Description = "Save job",
            OperationId = "SaveAsync")]
        public async Task<IActionResult> SaveAsync(JobCreateDto request)
        {
            return CreateActionResult(await _jobService.SaveAsync(request));
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "GetAllAsync",
            Description = "GetAll jobs",
            OperationId = "GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return CreateActionResult(await _jobService.GetAllAsync());
        }

        [HttpGet("jobId")]
        [SwaggerOperation(
            Summary = "GetByIdAsync",
            Description = "GetById job",
            OperationId = "GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(string jobId)
        {
            return CreateActionResult(await _jobService.GetByIdAsync(jobId));
        }

        [HttpPost("jobId")]
        [SwaggerOperation(
            Summary = "DeleteByIdAsync",
            Description = "DeleteById job",
            OperationId = "DeleteByIdAsync")]
        public async Task<IActionResult> DeleteByIdAsync(string jobId)
        {
            return CreateActionResult(await _jobService.DeleteByIdAsync(jobId));
        }
    }
}
