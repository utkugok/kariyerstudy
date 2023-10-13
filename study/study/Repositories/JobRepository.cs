using Elastic.Clients.Elasticsearch;
using study.Models;
using study.Repositories.Interfaces;
using System.ComponentModel.Design;
using System.Net;

namespace study.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "jobs";

        public JobRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Job> SaveAsync(Job newJob)
        {
            newJob.CreatedAt = DateTime.Now;

            var response = await _client.IndexAsync(newJob, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));

            if (!response.IsValidResponse)
            {
                throw new Exception($"create job error. {response.DebugInformation}");
            }

            newJob.Id = response.Id;

            return newJob;
        }

        public async Task<IReadOnlyCollection<Job>> GetAllAsync()
        {
            var response = await _client.SearchAsync<Job>(s => s
            .Index(indexName)
            .From(0)
            .Size(10000)
            .Query(q => q.
                Term(t => t.Status, 1))); // get active jobs

            if (response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"job getall error. {response.DebugInformation}");
            }

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents;
        }

        public async Task<Job?> GetByIdAsync(string jobId)
        {
            var response = await _client.GetAsync<Job>(jobId, idx => idx.Index(indexName));

            if (response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"Check your job id: {jobId}");
            }

            response.Source.Id = jobId;

            return response.Source;
        }

        public async Task<bool> DeleteByIdAsync(string jobId)
        {
            var response = await _client.DeleteAsync(indexName, jobId);

            if (response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
            {
                return false;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"job deleteById error. {response.DebugInformation}");
            }

            return true;
        }
    }
}