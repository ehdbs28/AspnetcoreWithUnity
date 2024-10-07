using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private static readonly List<Player> Players = new List<Player>();

    [HttpGet]
    public ActionResult<IEnumerable<Player>> GetPlayers()
    {
        return Players;
    }

    [HttpPost]
    public ActionResult<Player> AddPlayer([FromBody] Player newPlayer)
    {
        newPlayer.Id = Players.Count + 1;
        Players.Add(newPlayer);
        return newPlayer;
    }
}