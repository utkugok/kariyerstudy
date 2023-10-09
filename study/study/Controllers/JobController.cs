using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using study.DTOs;
using study.Services;
using study.Services.Interfaces;

namespace study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : BaseController
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(JobCreateDto request)
        {
            return CreateActionResult(await _jobService.SaveAsync(request));
        }
    }
}
