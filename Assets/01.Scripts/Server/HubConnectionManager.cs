using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public enum HubType
{
    GameHub,
    PlayerHub,
}

public class HubConnectionManager : MonoSingleton<HubConnectionManager>, IDisposable
{
    private readonly Dictionary<HubType, HubConnection> _hubConnections = new Dictionary<HubType, HubConnection>();
    private readonly string _serverUrl = "http://localhost:5162/";
    
    private object _locker = new object();

    public async Task Init()
    {
        foreach (HubType type in Enum.GetValues(typeof(HubType)))
        {
            var newHubConnection = new HubConnectionBuilder()
                .WithUrl($"{_serverUrl}{type.ToString()}")
                .Build();
            _hubConnections[type] = newHubConnection;
        }
    }

    public async Task ConnectToServer()
    {
        try
        {
            foreach (HubType type in Enum.GetValues(typeof(HubType)))
            {
                await _hubConnections[type].StartAsync();
            }
            Debug.Log("Connection to hub");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    public HubConnection GetHubConnection(HubType type)
    {
        lock (_locker)
        {
            if (!_hubConnections.TryGetValue(type, out var hubConnection))
            {
                Debug.LogWarning($"{type.ToString()} is null.");
                return null;
            }

            if (hubConnection.State != HubConnectionState.Connected)
            {
                Debug.LogWarning($"{type.ToString()} is not connected.");
            }

            return hubConnection;
        }
    }

    public void OnDestroy()
    {
        Dispose();
    }

    public async void Dispose()
    {
        foreach (var hubConnection in _hubConnections.Values)
        {
            await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
        }
        _hubConnections.Clear();
    }
}