using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace signalr_aspnetcore
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.All.InvokeAsync("broadcastMessage", "system", $"{Context.ConnectionId} joined the conversation");
            return base.OnConnectedAsync();
        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.InvokeAsync("broadcastMessage", name, message);
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            Clients.All.InvokeAsync("broadcastMessage", "system", $"{Context.ConnectionId} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
    }
}