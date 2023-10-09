using Elastic.Clients.Elasticsearch;
using study.Models;
using study.Repositories.Interfaces;
using System.ComponentModel.Design;
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

            var response = await _client.IndexAsync(newCompany, x=>x.Index(indexName).Id(Guid.NewGuid().ToString()));

            if(!response.IsValidResponse)
            {
                throw new Exception($"create company error. {response.DebugInformation}");
            }

            newCompany.Id = response.Id;

            return newCompany;
        }

        public async Task<Company?> SearchByPhoneNumberAsync(string phoneNumber)
        {
            var check = await _client.SearchAsync<Company>(s => s.
            Index(indexName)
            .From(0)
            .Size(10)
            .Query(q => q.
                Term(t => t.PhoneNumber, phoneNumber)));

            if (!check.IsValidResponse)
            {
                throw new Exception($"search company error. {check.DebugInformation}");
            }

            return check.Documents.FirstOrDefault();
        }

        public async Task<Company?> GetByIDAsync(string id)
        {
            var response = await _client.GetAsync<Company>(id,idx=>idx.Index(indexName)) ;

            if (!response.IsValidResponse)
            {
                throw new Exception($"Check your company id: {id}");
            }
            return response.Source;
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
            var response = await _client.SearchAsync<Company>(s=>s
            .Index(indexName)
            .Query(q=>q
            .MatchAll()));

            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            if (!response.IsValidResponse)
            {
                throw new Exception($"company getall error. {response.DebugInformation}");
            }

            return response.Documents;
        }
    }
}
