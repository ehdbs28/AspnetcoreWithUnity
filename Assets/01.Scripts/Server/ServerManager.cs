using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class ServerManager : MonoSingleton<ServerManager>, IGameHubConnector
{
    public int MyUserId { get; private set; }    

    public void Init()
    {
        var gameHub = HubConnectionManager.Instance.GetHubConnection(HubType.GameHub);

        gameHub.On("LoginFailure", OnLoginFailure);
        gameHub.On<int>("LoginSuccess", OnLoginSuccess);
        gameHub.On<int>("LogOut", OnLogOut);
    }

    public bool IsOwner(int userId)
    {
        return MyUserId == userId;
    }

    public void OnLoginFailure()
    {
        //
    }

    public void OnLoginSuccess(int userId)
    {
        MyUserId = userId;
        PlayerManager.Instance.CreatePlayer(userId);
    }

    public void OnLogOut(int userId)
    {
        PlayerManager.Instance.DeletePlayer(userId);
    }
}