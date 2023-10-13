using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using study.DTOs;
using study.Services;
using study.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace study.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProhibitedWordsController : BaseController
    {
        private readonly IProhibitedWordsService _prohibitedWords;

        public ProhibitedWordsController(IProhibitedWordsService prohibitedWords)
        {
            _prohibitedWords = prohibitedWords;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "SaveAsync",
            Description = "Save prohibitedWord",
            OperationId = "SaveAsync")]
        public async Task<IActionResult> SaveAsync(ProhibitedWordCreateDto request)
        {
            return CreateActionResult(await _prohibitedWords.SaveAsync(request));
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "GetAllAsync",
            Description = "GetAll prohibitedWords",
            OperationId = "GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return CreateActionResult(await _prohibitedWords.GetAllAsync());
        }

        [HttpPost("prohibitedWordId")]
        [SwaggerOperation(
            Summary = "DeleteByIdAsync",
            Description = "DeleteById prohibitedWords",
            OperationId = "DeleteByIdAsync")]
        public async Task<IActionResult> DeleteByIdAsync(string prohibitedWordId)
        {
            return CreateActionResult(await _prohibitedWords.DeleteByIdAsync(prohibitedWordId));
        }
    }
}
