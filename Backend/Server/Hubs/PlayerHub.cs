using System.Numerics;
using Backend.DB;
using Backend.DB.Models;
using Backend.DB.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Backend.Server.Hubs;

public class PlayerHub : Hub
{
    private readonly ILogger<PlayerHub> _logger;
    private readonly CharacterServices _characterDbServices;

    public PlayerHub(ILogger<PlayerHub> logger, GameDBContext dbContext)
    {
        _logger = logger;
        _characterDbServices = new CharacterServices(dbContext);
    }

    public async Task CreatePlayer(int userId)
    {
        var character = await _characterDbServices.GetCharacterByUserIdAsync(userId);

        if (character == null)
        {
            var newCharacter = new Character
            {
                OwnerUserId = userId,
                Level = 0,
                LastPosition = Vector3.Zero
            };

            await _characterDbServices.AddCharacter(newCharacter);

            var characterData = JsonConvert.SerializeObject(newCharacter);
            await Clients.All.SendAsync("CreatePlayer", characterData);
        }
        else
        {
            var characterData = JsonConvert.SerializeObject(character);
            await Clients.All.SendAsync("CreatePlayer", characterData);
        }
        
        _logger.LogInformation($"Success Create Character.");
    }

    public async Task DeletePlayer(string characterData)
    {
        var character = JsonConvert.DeserializeObject<Character>(characterData);
        await _characterDbServices.UpdateCharacter(character);
        await Clients.All.SendAsync("DeletePlayer", character.OwnerUserId);
    }

    public async Task UpdatePlayerPosition(int userId, string newPositionJson)
    {
        await Clients.All.SendAsync("UpdatePosition", userId, newPositionJson);
    }
}