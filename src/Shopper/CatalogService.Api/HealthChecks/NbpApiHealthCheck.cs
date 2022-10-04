using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CatalogService.Api.HealthChecks
{
    public class NbpApiHealthCheck : IHealthCheck
    {
        private const string Uri = "https://api.nbp.pl/api/exchangerates/tables/a?format=json";

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
