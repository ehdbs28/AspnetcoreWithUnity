using System;
using Microsoft.AspNetCore.SignalR.Client;
using TMPro;
using UnityEngine;

public class ServerManager : MonoSingleton<ServerManager>
{
    private HubConnection _gameHub;
    private readonly string _serverUrl = "http://localhost:5162/";
    private string _playerId;

    private bool _isSet = false;

    [Header("For Test")] 
    [SerializeField] private TextMeshProUGUI _logText;

    private async void Start()
    {
        _gameHub = new HubConnectionBuilder()
            .WithUrl($"{_serverUrl}gameHub")
            .Build();

        _gameHub.On<string, string>("ReceiveMessage", (clientId, message) =>
        {
            var log = $"{clientId}: {message}\n";
            Debug.Log(log);
            _logText.text += log;
        });

        _gameHub.On<string>("PlayerJoined", playerId =>
        {
            if (_isSet)
            {
                return;
            }
            
            _playerId = playerId;
            _isSet = true;
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

        await _gameHub.InvokeAsync("JoinGame");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessage(_playerId, "SendMessage to client");
        }
    }

    public async void SendMessage(string playerId, string message)
    {
        try
        {
            await _gameHub.InvokeAsync("SendMessage", playerId, message);
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