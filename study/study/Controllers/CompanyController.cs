using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using study.DTOs;
using study.Services;

namespace study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : BaseController
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(CompanyCreateDto request)
        {
            return  CreateActionResult(await _companyService.SaveAsync(request));
        }
    }
}
