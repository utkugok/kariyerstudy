using Elastic.Clients.Elasticsearch;
using study.Models;
using study.Repositories.Interfaces;
using System.ComponentModel.Design;
using System.Net;
using System.Text.Json;

namespace study.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "companies";

        public CompanyRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Company> SaveAsync(Company newCompany)
        {
            newCompany.CreatedAt = DateTime.Now;

            var response = await _client.IndexAsync(newCompany, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));

            if (!response.IsValidResponse)
            {
                throw new Exception($"create company error. {response.DebugInformation}");
            }

            newCompany.Id = response.Id;

            return newCompany;
        }

        public async Task<Company?> SearchByPhoneNumberAsync(string phoneNumber)
        {
            var response = await _client.SearchAsync<Company>(s => s.
            Index(indexName)
            .Query(q => q.
                Term(t => t.PhoneNumber, phoneNumber)));

            if (response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"search company error. {response.DebugInformation}");
            }

            return response.Documents.FirstOrDefault();
        }

        public async Task<bool> UpdateCompanyJobPostLimitAsync(string companyId, Company company)
        {
            company.JobPostLimit -= 1;

            var response = await _client.UpdateAsync<Company, Company>(indexName, companyId, u => u.Doc(company));

            if (!response.IsValidResponse)
            {
                throw new Exception($"company update error. {response.DebugInformation}");
            }

            return response.IsSuccess();
        }

        public async Task<IReadOnlyCollection<Company>> GetAllAsync()
        {
            try
            {
                var response = await _client.SearchAsync<Company>(s => s
                .Index(indexName)
                .From(0)
                .Size(10000)
                .Query(q => q
                .MatchAll()));

                if(response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
                {
                    return null;
                }

                if (!response.IsValidResponse)
                {
                    throw new Exception($"company getall error. {response.DebugInformation}");
                }


                foreach (var hit in response.Hits)
                {
                    hit.Source.Id = hit.Id;
                }

                return response.Documents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<Company?> GetByIdAsync(string companyId)
        {
            var response = await _client.GetAsync<Company>(companyId, idx => idx.Index(indexName));

            if (response.ApiCallDetails.HttpStatusCode is (int?)HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"Check your company id: {companyId}");
            }

            response.Source.Id = companyId;

            return response.Source;
        }
    }
}
