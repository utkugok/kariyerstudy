using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace study.Extensions
{
    public static class ElasticsearchExt
    {
        public static void AddElastic ( this IServiceCollection services, IConfiguration configuration)
        {
            var userName = configuration.GetSection("Elastic")["Username"];
            var password = configuration.GetSection("Elastic")["Password"];
            var url = configuration.GetSection("Elastic")["Url"];
            var settings = new ElasticsearchClientSettings(new Uri(url!)).Authentication(new BasicAuthentication(userName!, password!));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client); 
        }
    }
}
