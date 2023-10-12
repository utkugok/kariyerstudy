using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using study.DTOs;
using study.Models;
using study.Services;
using study.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : BaseController
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "SaveAsync",
            Description = "Save company",
            OperationId = "SaveAsync")]
        public async Task<IActionResult> SaveAsync(CompanyCreateDto request)
        {
            return CreateActionResult(await _companyService.SaveAsync(request));
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "GetAllAsync",
            Description = "GetAll companies",
            OperationId = "GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return CreateActionResult(await _companyService.GetAllAsync());
        }

        [HttpGet("companyId")]
        [SwaggerOperation(
            Summary = "GetByIdAsync",
            Description = "GetById company",
            OperationId = "GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(string companyId)
        {
            return CreateActionResult(await _companyService.GetByIdAsync(companyId));
        }
    }
}
