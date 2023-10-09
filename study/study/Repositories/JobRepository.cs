using Elastic.Clients.Elasticsearch;
using study.Models;
using study.Repositories.Interfaces;

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
    }
}