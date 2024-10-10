using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public enum HubType
{
    GameHub,
}

public class HubConnectionManager : MonoSingleton<HubConnectionManager>, IDisposable
{
    private readonly Dictionary<HubType, HubConnection> _hubConnections = new Dictionary<HubType, HubConnection>();
    private readonly string _serverUrl = "http://localhost:5162/";
    
    private object _locker = new object();

    public HubConnection GetHubConnection(HubType type)
    {
        lock (_locker)
        {
            if (!_hubConnections.ContainsKey(type) || _hubConnections[type] == null)
            {
                var newHubConnection = new HubConnectionBuilder()
                    .WithUrl($"{_serverUrl}{type.ToString()}")
                    .Build();
                
                _hubConnections[type] = newHubConnection;
                Debug.Log($"Create New Hub Connection: {type.ToString()}");
            }
            
            return _hubConnections[type];
        }
    }

    public async Task StartConnection(HubType type)
    {
        var hubConnection = GetHubConnection(type);
        if (hubConnection.State != HubConnectionState.Connected)
        {
            await hubConnection.StartAsync();
        }
    }

    public async Task StopConnection(HubType type)
    {
        var hubConnection = GetHubConnection(type);
        if (hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.StopAsync();
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