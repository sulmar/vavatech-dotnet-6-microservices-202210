using CatalogService.Api.Commands;
using CatalogService.Domain;
using MediatR;

namespace CatalogService.Api.Handlers
{
    public class AddProductToRepositoryHandler : INotificationHandler<AddProductCommand>
    {
        private readonly IProductRepository productRepository;

        public AddProductToRepositoryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Task Handle(AddProductCommand notification, CancellationToken cancellationToken)
        {
            productRepository.Add(notification.Product);

            return Task.CompletedTask;            
        }
    }
}
