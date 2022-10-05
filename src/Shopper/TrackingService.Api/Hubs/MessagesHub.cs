using Microsoft.AspNetCore.SignalR;
using TrackingService.Domain;

namespace TrackingService.Api.Hubs
{

    public class MessagesHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connected {Context.ConnectionId}");

            return base.OnConnectedAsync();
        }

        private async Task Join(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }

        public async Task SendMessage(Message message)
        {
            // All = Caller + Others
            await this.Clients.Others.SendAsync("YouHaveGotMessage", message);

            Console.WriteLine($"{message} Sent.");
        }

        public async Task SendMessage(Message message, string room)
        {
            // All = Caller + Others
            await this.Clients.Group(room).SendAsync("YouHaveGotMessage", message);

            Console.WriteLine($"{message} Sent.");
        }

        public async Task Ping()
        {
            await this.Clients.Caller.SendAsync("Pong");
        }

    }
}
