using System;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ServerManager : MonoSingleton<ServerManager>
{
    private HubConnection _gameHub;

    private readonly string _serverUrl = "http://localhost:5162/";

    private async void Start()
    {
        _gameHub = new HubConnectionBuilder()
            .WithUrl($"{_serverUrl}gameHub")
            .Build();

        _gameHub.On<string, string>("ReceiveMessage", (clientId, message) =>
        {
            Debug.Log($"{clientId}: {message}");
        });

        try
        {
            await _gameHub.StartAsync();
            Debug.Log("Connection to hub");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    public async void SendMessage(string user, string message)
    {
        try
        {
            await _gameHub.InvokeAsync("SendMessage", user, message);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error sending message: {ex.Message}");
        }
    }

    private async void OnDestroy()
    {
        await _gameHub.StopAsync();
        await _gameHub.DisposeAsync();
    }
}