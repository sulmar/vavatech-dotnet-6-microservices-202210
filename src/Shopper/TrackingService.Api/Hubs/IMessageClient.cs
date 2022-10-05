using TrackingService.Domain;

namespace TrackingService.Api.Hubs
{
    public interface IMessageClient
    {
        Task YouHaveGotMessage(Message message);
        Task Pong();
    }
}
