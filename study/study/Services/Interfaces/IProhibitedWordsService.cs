using study.DTOs;

namespace study.Services.Interfaces
{
    public interface IProhibitedWordsService
    {
        Task<ResponseDto<ProhibitedWordDto>> SaveAsync(ProhibitedWordCreateDto request);

        Task<ResponseDto<List<ProhibitedWordDto>>> GetAllAsync();

        Task<ResponseDto<bool>> DeleteByIdAsync(string prohibitedWordId);

        Task<bool> CheckProhibitedWordInDescriptionAsync(string description);

        
    }
}
