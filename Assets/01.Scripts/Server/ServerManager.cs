using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class ServerManager : MonoSingleton<ServerManager>, IGameHubConnector
{
    public string MyClientId { get; private set; }    

    public void Init()
    {
        var gameHub = HubConnectionManager.Instance.GetHubConnection(HubType.GameHub);

        gameHub.On<string>("ClientConnect", OnClientConnect);
        gameHub.On<string, string>("PlayerJoined", OnPlayerJoined);
        gameHub.On<string>("PlayerLeft", OnPlayerLeft);
    }

    public bool IsOwner(string clientId)
    {
        return MyClientId == clientId;
    }

    public void OnClientConnect(string clientId)
    {
        if (string.IsNullOrEmpty(MyClientId))
        {
            MyClientId = clientId;
        }
    }

    public void OnPlayerJoined(string clientId, string nickName)
    {
        try
        {
            PlayerManager.Instance.CreatePlayer(clientId, nickName);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }

    public void OnPlayerLeft(string clientId)
    {
        try
        {
            PlayerManager.Instance.DestroyPlayer(clientId);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }
}