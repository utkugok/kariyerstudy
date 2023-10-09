using study.Models;
using System.Net;

namespace study.DTOs
{
    public record ProhibitedWordCreateDto(string prohibitedWord)
    {
        public ProhibitedWord CreateProhibitedWord()
        {
            return new ProhibitedWord()
            {
                Word = prohibitedWord.Trim()
            };
        }
    }
}