using CatalogService.Api.Commands;
using CatalogService.Domain;
using MediatR;

namespace CatalogService.Api.Handlers
{
    public class AddProductToRepositoryHandler : INotificationHandler<AddProductCommand>
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<AddProductToRepositoryHandler> logger;

        public AddProductToRepositoryHandler(IProductRepository productRepository, ILogger<AddProductToRepositoryHandler> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        public Task Handle(AddProductCommand notification, CancellationToken cancellationToken)
        {
            productRepository.Add(notification.Product);

            // zła praktyka
            // logger.LogInformation($"{notification.Product.Name} Added.");

            // dobra praktyka
            logger.LogInformation("Name={Name} Color={Color} Added.", notification.Product.Name, notification.Product.Color);

            return Task.CompletedTask;            
        }
    }
}
