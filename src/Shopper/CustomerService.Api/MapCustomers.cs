using CustomerService.Domain;

namespace CustomerService.Api
{
    public static class MapExtensions
    {
        public static IEndpointRouteBuilder MapCustomers(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/customers", (ICustomerRepository repository) => Results.Ok(repository.Get()));
            app.MapGet("api/customers/{id}", (ICustomerRepository repository, int id) => Results.Ok(repository.Get(id)));

            return app;

        }
    }
}
