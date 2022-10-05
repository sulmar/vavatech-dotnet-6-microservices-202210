using Microsoft.AspNetCore.SignalR;
using TrackingService.Domain;

namespace TrackingService.Api.Hubs
{
    public class StrongTypedMessagesHub : Hub<IMessageClient>
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected {Context.ConnectionId}");

            return base.OnConnectedAsync();
        }

        public async Task SendMessage(Message message)
        {
            await this.Clients.Others.YouHaveGotMessage(message);
        }

        public async Task Ping()
        {
            await this.Clients.Caller.Pong();
        }
    }
}
