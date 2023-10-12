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

        public string Position { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int DurationInDays { get; set; }

        public int Status { get; set; } // Status, 1 aktif, 0 pasif

        public int Quality { get; set; }

        public string? Benefits { get; set; }

        public string? WorkType { get; set; }

        public decimal? Salary { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CompanyId { get; set; } = null!;

        public JobDto CreateDto()
        {
            return new JobDto(Id, CompanyId, Position, Description, DurationInDays, Quality, Benefits, WorkType, Salary, CreatedAt);
        }
    }
}
