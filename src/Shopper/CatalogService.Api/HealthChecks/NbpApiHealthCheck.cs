using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.HealthChecks
{

    public class NbpApiOptions
    {
        public string Url { get; set; }
        public string Table { get; set; }
        public string Format { get; set; }
    }

    public class NbpApiHealthCheck : IHealthCheck
    {
        private const string Uri = "https://api.nbp.pl/api/exchangerates/tables/a?format=json";

        private readonly NbpApiOptions options;

        public NbpApiHealthCheck(IOptions<NbpApiOptions> options)
        {
            this.options = options.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            using var client = new HttpClient();

            var response = await client.GetAsync(Uri);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy();
            }
            else
            {
                return HealthCheckResult.Degraded();
            }
            
        }
    }
}
