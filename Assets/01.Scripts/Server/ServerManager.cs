using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class ServerManager : MonoSingleton<ServerManager>, IGameHubConnector
{
    public int MyUserId { get; private set; }    
    
    public bool IsLogIn { get; private set; }
    public bool IsLogOut { get; set; }

    public void Init()
    {
        IsLogIn = false;
        IsLogOut = false;
        
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
        Debug.Log("로그인 실패");
    }

    public void OnLoginSuccess(int userId)
    {
        Debug.Log("로그인 성공");
        IsLogIn = true;
        MyUserId = userId;
        PlayerManager.Instance.CreatePlayer(userId);
    }

    public void OnLogOut(int userId)
    {
        Debug.Log("로그아웃");
        PlayerManager.Instance.DeletePlayer(userId);
    }
}