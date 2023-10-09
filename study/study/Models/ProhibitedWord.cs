using study.DTOs;
using System.Net;

namespace study.Models
{
    public class ProhibitedWord
    {
        public string Id { get; set; }
        public string Word { get; set; }

        public DateTime CreatedAt { get; set; }


        public ProhibitedWordDto CreateDto()
        {
            return new ProhibitedWordDto(Id, Word, CreatedAt);
        }
    }
}
