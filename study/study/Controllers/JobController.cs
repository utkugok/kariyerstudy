using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using study.DTOs;
using study.Services;

namespace study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : BaseController
    {
        private readonly JobService _jobService;

        public JobController(JobService jobService)
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
