using System.Numerics;
using Backend.Server.Core;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Server.Hubs;

public class PlayerHub : Hub
{
    private readonly ILogger<PlayerHub> _logger;

    public PlayerHub(ILogger<PlayerHub> logger)
    {
        _logger = logger;
    }

    public Vector3 GetPlayerPositionInServer(string clientId)
    {
        var player = PlayerManager.Instance.GetPlayer(clientId);

        if (player == null)
        {
            _logger.LogWarning($"Cant Find Player: {clientId}");
            return Vector3.Zero;
        }

        return player.Position;
    }

    public async Task UpdatePlayerPosition(string clientId, Vector3 newPosition)
    {
        var player = PlayerManager.Instance.GetPlayer(clientId);

        if (player == null)
        {
            _logger.LogWarning($"Cant Find Player: {clientId}");
            return;
        }

        await Clients.AllExcept(clientId).SendAsync("UpdatePosition", clientId, newPosition);
        player.UpdatePosition(newPosition);
    }
}