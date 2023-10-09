using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using study.DTOs;
using study.Models;
using study.Services;
using study.Services.Interfaces;
using System.Collections.Generic;

namespace study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(CompanyCreateDto request)
        {
            return  CreateActionResult(await _companyService.SaveAsync(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return CreateActionResult(await _companyService.GetAllAsync());
        }


    }
}
