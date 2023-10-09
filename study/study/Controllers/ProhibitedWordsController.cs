using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using study.DTOs;
using study.Services;
using study.Services.Interfaces;

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
        public async Task<IActionResult> Save(ProhibitedWordCreateDto request)
        {
            return CreateActionResult(await _prohibitedWords.SaveAsync(request));
        }

        [HttpGet]
        [OutputCache]
        public async Task<IActionResult> GetAllAsync()
        {
            return CreateActionResult(await _prohibitedWords.GetAllAsync());
        }

    }
}
