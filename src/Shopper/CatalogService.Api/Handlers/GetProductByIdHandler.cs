using CatalogService.Api.Queries;
using CatalogService.Domain;
using MediatR;

namespace CatalogService.Api.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = _productRepository.Get(request.id);

            return Task.FromResult(product);
        }
    }
}
