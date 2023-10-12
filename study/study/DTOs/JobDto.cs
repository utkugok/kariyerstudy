using study.Models;
using System.ComponentModel.DataAnnotations;

namespace study.DTOs
{
    public record JobDto(string id, string companyId, string position, string description, int durationInDays, int quality, string? benefits, string? workType, decimal? salary, DateTime createdAt)
    {
    }
}
