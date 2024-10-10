using System.Collections.Concurrent;
using Backend.Server.Core;
using Backend.Server.Object;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Server.Hubs;

public class GameHub : Hub
{
    private static readonly Dictionary<string, Player> Players = new Dictionary<string, Player>();

    private readonly ILogger<GameHub> _logger;

    public GameHub(ILogger<GameHub> logger)
    {
        _logger = logger;
    }

    public async Task ConnectPlayer(string nickName)
    {
        var clientId = Context.ConnectionId;

        PlayerManager.Instance.ConnectPlayer(clientId, nickName);
        
        await Clients.All.SendAsync("PlayerJoined", clientId, nickName);
        _logger.LogInformation($"Successfully Connect Player: {clientId}");
    }

    public async Task DisconnectPlayer(string clientId)
    {
        PlayerManager.Instance.DisconnectPlayer(clientId);
        await Clients.All.SendAsync("PlayerLeft", clientId);
        _logger.LogInformation($"Disconnect Player: {clientId}");
    }
}