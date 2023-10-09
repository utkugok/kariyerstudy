using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Fluent;
using Microsoft.AspNetCore.Http.HttpResults;
using study.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Net;

namespace study.Models
{
    public class Job
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Pozisyon zorunlu bir alan.")]
        public string Position { get; set; } = null!;

        [Required(ErrorMessage = "İlan açıklaması zorunlu bir alan.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "İlanın yayında kalma süresi zorunlu bir alan.")]
        public int DurationInDays { get; set; }

        public int Status { get; set; } // Status, 1 aktif, 0 pasif

        public int Quality { get; set; }

        public string? Benefits { get; set; }

        public string? WorkType { get; set; }

        public decimal? Salary { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CompanyId { get; set; } = null!;

        //public Company Company { get; set; }

        public JobDto CreateDto()
        {
            return new JobDto(Id, Position, Description, DurationInDays, Quality, Benefits, WorkType, Salary, CreatedAt);
        }
    }
}
