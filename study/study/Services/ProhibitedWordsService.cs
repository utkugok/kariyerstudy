using study.DTOs;
using study.Repositories.Interfaces;
using study.Repositories;
using study.Services.Interfaces;
using System.Net;
using study.Models;
using System.Linq;

namespace study.Services
{
    public class ProhibitedWordsService : IProhibitedWordsService
    {
        private readonly IProhibitedWordsRepository _prohibitedWordsRepository;
        private readonly ICacheService _cacheService;

        public ProhibitedWordsService(IProhibitedWordsRepository prohibitedWordsRepository, ICacheService cacheService)
        {
            _prohibitedWordsRepository = prohibitedWordsRepository;
            _cacheService = cacheService;
        }
        public async Task<ResponseDto<ProhibitedWordDto>> SaveAsync(ProhibitedWordCreateDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.prohibitedWord))
                {
                    return ResponseDto<ProhibitedWordDto>.Fail("Yasaklı kelime boş olamaz.", HttpStatusCode.BadRequest);
                }
                var prohibitedWord = request.CreateProhibitedWord();

                var cacheWords = _cacheService.GetData<IEnumerable<ProhibitedWord>>("prohibitedwords")?.ToList();

                var searchProhibitedWord = cacheWords?.FirstOrDefault(q => q.Word == prohibitedWord.Word);

                if (searchProhibitedWord is not null)
                {
                    throw new Exception($"'{prohibitedWord.Word}'yasaklı kelimesi zaten kayıtlıdır.");
                }

                var responseProhibitedWord = await _prohibitedWordsRepository.SaveAsync(prohibitedWord);

                if (cacheWords is null)
                {
                    cacheWords = new List<ProhibitedWord>();
                    cacheWords.Add(responseProhibitedWord);
                }
                else
                {
                    cacheWords.Add(responseProhibitedWord);
                }
                _cacheService.SetData<IEnumerable<ProhibitedWord>>("prohibitedwords", cacheWords, DateTimeOffset.MaxValue);

                return ResponseDto<ProhibitedWordDto>.Success(responseProhibitedWord.CreateDto(), HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ResponseDto<ProhibitedWordDto>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto<List<ProhibitedWordDto>>> GetAllAsync()
        {
            try
            {
                var prohibitedWords = await _prohibitedWordsRepository.GetAllAsync();

                var prohibitedWordListDto = prohibitedWords.Select(x => new ProhibitedWordDto(x.Id, x.Word, x.CreatedAt)).ToList();

                return ResponseDto<List<ProhibitedWordDto>>.Success(prohibitedWordListDto, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ResponseDto<List<ProhibitedWordDto>>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public bool CheckProhibitedWordInDescription(string description)
        {
            var cacheWords = _cacheService.GetData<IEnumerable<ProhibitedWord>>("prohibitedwords")?.ToList();
            var isProhibitedWord = new ProhibitedWordCreateDto(description).CreateProhibitedWord();

            if (cacheWords is not null)
            {
                foreach (var cacheword in cacheWords)
                {
                    if(isProhibitedWord.Word.Contains(cacheword.Word))
                    {
                        return true;                    
                    }
                }
            }

            return false;
        }
    }
}
