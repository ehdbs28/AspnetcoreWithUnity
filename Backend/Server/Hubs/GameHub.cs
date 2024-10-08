using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Server.Hubs;

public class GameHub : Hub
{
    private static int _connectedPlayerCount = 0;
    
    public async Task JoinGame()
    {
        Console.WriteLine(_connectedPlayerCount.ToString());
        await Clients.All.SendAsync("PlayerJoined", _connectedPlayerCount.ToString());
        _connectedPlayerCount++;
    }
    
    public async Task SendMessage(string clientId, string message)
    {
        Console.WriteLine($"{clientId}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", clientId, message);
    }
}