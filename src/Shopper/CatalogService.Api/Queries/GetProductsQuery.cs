using CatalogService.Domain;
using MediatR;

namespace CatalogService.Api.Queries
{
    public record GetProductsQuery() : IRequest<IEnumerable<Product>>;
    
}
