using System.Collections.Concurrent;
using Backend.Server.Core;
using Backend.Server.Object;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Server.Hubs;

public class GameHub : Hub
{
    private readonly ILogger<GameHub> _logger;
    private static int _userCount;

    public GameHub(ILogger<GameHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _userCount++;
        await Clients.All.SendAsync("ClientConnect", _userCount.ToString());
        await base.OnConnectedAsync();
    }

    public async Task ConnectPlayer(string nickName)
    {
        var clientId = _userCount.ToString();

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