using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using study.Models;

namespace study.Extensions
{
    public static class ElasticsearchExt
    {
        public static void AddElastic ( this IServiceCollection services, IConfiguration configuration)
        {
            var userName = configuration.GetSection("Elastic")["Username"];
            var password = configuration.GetSection("Elastic")["Password"];
            var url = configuration.GetSection("Elastic")["Url"];
            var settings = new ElasticsearchClientSettings(new Uri(url!))
                .Authentication(new BasicAuthentication(userName!, password!))
                .DefaultMappingFor<Company>(i => i
                    .IndexName("companies")
                    .IdProperty(p => p.PhoneNumber))
                .DefaultMappingFor<Job>(i => i
                    .IndexName("jobs")
                    .IdProperty(p => p.Id))
                .DefaultMappingFor<ProhibitedWord>(i => i
                    .IndexName("prohibitedwords")
                    .IdProperty(p => p.Word));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client); 
        }
    }
}
