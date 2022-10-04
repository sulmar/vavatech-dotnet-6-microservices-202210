using CatalogService.Api.Commands;
using CatalogService.Domain;
using MediatR;

namespace CatalogService.Api.Handlers
{
    public class SendProductHandler : INotificationHandler<AddProductCommand>
    {
        private readonly IMessageService messageService;

        public SendProductHandler(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public Task Handle(AddProductCommand notification, CancellationToken cancellationToken)
        {
            messageService.Send(notification.Product);

            return Task.CompletedTask;
        }
    }
}
