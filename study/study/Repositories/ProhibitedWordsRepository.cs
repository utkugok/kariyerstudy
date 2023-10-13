using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Reindex;
using Elastic.Transport.Products.Elasticsearch;
using study.DTOs;
using study.Models;
using study.Repositories;
using study.Repositories.Interfaces;
using System.Net;

namespace study.Repositories
{
    public class ProhibitedWordsRepository : IProhibitedWordsRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "prohibitedwords";

        public ProhibitedWordsRepository(ElasticsearchClient client)
        {
            _client = client;
        }
        public async Task<ProhibitedWord> SaveAsync(ProhibitedWord newProhibitedWord)
        {
            newProhibitedWord.CreatedAt = DateTime.Now;

            var response = await _client.IndexAsync(newProhibitedWord, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));

            if (!response.IsValidResponse)
            {
                throw new Exception($"create prohibited word error. {response.DebugInformation}");
            }

            newProhibitedWord.Id = response.Id;

            return newProhibitedWord;
        }

        public async Task<ProhibitedWord?> SearchByProhibitedWordAsync(string prohibitedWord)
        {
            var response = await _client.SearchAsync<ProhibitedWord>(s => s.
            Index(indexName)
            .Query(q => q.
                Term(t => t.Word, prohibitedWord)));

            if (response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"search prohibitedword error. {response.DebugInformation}");
            }

            return response.Documents.FirstOrDefault();
        }

        public async Task<IReadOnlyCollection<ProhibitedWord>> GetAllAsync()
        {
            var response = await _client.SearchAsync<ProhibitedWord>(s => s
           .Index(indexName)
           .From(0)
           .Size(10000)
           .Query(q => q
           .MatchAll()));

            if (response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"prohibitedwords getall error. {response.DebugInformation}");
            }

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents;
        }

        public async Task<bool> DeleteByIdAsync(string prohibitedWordId)
        {
            var response = await _client.DeleteAsync(indexName, prohibitedWordId);

            if (response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
            {
                return true;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"prohibitedword delete error. {response.DebugInformation}");
            }

            return true;
        }
    }
}