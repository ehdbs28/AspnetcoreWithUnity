using System.Numerics;
using Backend.Server.Core;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

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

    public async Task UpdatePlayerPosition(string clientId, string newPositionJson)
    {
        var player = PlayerManager.Instance.GetPlayer(clientId);

        if (player == null)
        {
            _logger.LogWarning($"Cant Find Player: {clientId}");
            return;
        }

        var newPosition = JsonConvert.DeserializeObject<Vector3>(newPositionJson);

        await Clients.All.SendAsync("UpdatePosition", clientId, newPositionJson);
        player.UpdatePosition(newPosition);
        
        _logger.LogInformation($"Update Position: {clientId} to ({newPosition.X}, {newPosition.Y}, {newPosition.Z})");
    }
}