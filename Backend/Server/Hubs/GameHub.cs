using Microsoft.AspNetCore.SignalR;

namespace Backend.Server.Hubs;

public class GameHub : Hub
{
    public async Task SendMessage(string clientId, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", clientId, message);
    }
}