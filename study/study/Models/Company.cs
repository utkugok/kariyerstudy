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

        public string PhoneNumber { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int JobPostLimit { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Job>? Job { get; set; }

        
        public CompanyDto CreateDto()
        {
            return new CompanyDto(Id, PhoneNumber, CompanyName, Address, JobPostLimit, CreatedAt, null);
        }
    }
}
