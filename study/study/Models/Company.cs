using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using Elastic.Clients;
using Elastic.Clients.Elasticsearch.IndexManagement;
using study.DTOs;

namespace study.Models
{
    public class Company
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Telefon numarası zorunlu bir alan.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Firma adı zorunlu bir alan.")]
        public string CompanyName { get; set; } = null!;

        [Required(ErrorMessage = "Adres zorunlu bir alan.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Yayınlayabileceği ilan hakkı sayısı zorunlu bir alan.")]
        public int JobPostLimit { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Job>? Job { get; set; }

        
        public CompanyDto CreateDto()
        {
            return new CompanyDto(Id, PhoneNumber, CompanyName, Address, JobPostLimit, CreatedAt, null);
        }
    }
}
