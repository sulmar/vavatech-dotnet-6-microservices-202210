using CatalogService.Domain;
using MediatR;

namespace CatalogService.Api.Queries
{
    public record GetProductByIdQuery(int id) : IRequest<Product>;
    
}
