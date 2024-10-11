using System.Collections.Concurrent;
using Backend.DB;
using Backend.DB.Services;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Server.Hubs;

public class GameHub : Hub
{
    private readonly ILogger<GameHub> _logger;
    private readonly UserServices _userDbServices;
    
    public GameHub(ILogger<GameHub> logger, GameDBContext dbContext)
    {
        _logger = logger;
        _userDbServices = new UserServices(dbContext);
    }

    public async Task<string> GetUserName(int userId)
    {
        var user = await _userDbServices.GetUserByIdAsync(userId);
        return user == null ? "" : user.UserName;
    }

    public async Task<bool> Login(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            await Clients.Client(Context.ConnectionId).SendAsync("LoginFailure");
            return false;
        }
        
        var user = await _userDbServices.GetUserByUserNameAsync(userName);
        if (user == null)
        {
            user = await _userDbServices.AddUser(userName, password);
            await Clients.Client(Context.ConnectionId).SendAsync("LoginSuccess", user.Id);
        }
        else
        {
            if (user.Password != password)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("LoginFailure");
                return false;
            }
            await Clients.Client(Context.ConnectionId).SendAsync("LoginSuccess", user.Id);
        }
        
        var currentTime = DateTime.Now;
        _logger.LogInformation($"({currentTime:hh:mm:ss}) Login User: {userName}");

        return true;
    }

    public async Task LogOut(int id)
    {
        var user = await _userDbServices.GetUserByIdAsync(id);

        if (user == null)
        {
            return;
        }

        await Clients.Client(Context.ConnectionId).SendAsync("LogOut", id);
        
        var currentTime = DateTime.Now;
        _logger.LogInformation($"({currentTime:hh:mm:ss}) LogOut User: {user.UserName}");
    }
}