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

        public async Task<bool> CheckProhibitedWordInDescriptionAsync(string description)
        {
            var prohibitedWords = _cacheService.GetData<IEnumerable<ProhibitedWord>>("prohibitedwords")?.ToList();
            var isProhibitedWord = new ProhibitedWordCreateDto(description).CreateProhibitedWord();

            if (prohibitedWords is null)
            {
                var prohibitedWordsFromRepo = await _prohibitedWordsRepository.GetAllAsync();

                if(prohibitedWordsFromRepo is not null)
                {
                    prohibitedWords = prohibitedWordsFromRepo?.ToList();

                    _cacheService.SetData<IEnumerable<ProhibitedWord>>("prohibitedwords", prohibitedWords, DateTimeOffset.MaxValue);
                }
            }

            if(prohibitedWords is not null)
            {
                foreach (var prohibitedWord in prohibitedWords)
                {
                    if (isProhibitedWord.Word.Contains(prohibitedWord.Word))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(string prohibitedWordId)
        {
            try
            {
                var isSuccess = await _prohibitedWordsRepository.DeleteByIdAsync(prohibitedWordId);
                
                var prohibitedWords = _cacheService.RemoveData(prohibitedWordId);

                return ResponseDto<bool>.Success(isSuccess, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Fail(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
