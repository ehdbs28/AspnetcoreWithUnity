using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : MonoSingleton<PlayerManager>, IPlayerHubConnector
{
    [SerializeField] private MainPlayer _ownerPlayerPrefab;
    [SerializeField] private Player _playerPrefab;
    
    private readonly Dictionary<string, Player> _connectedPlayers = new Dictionary<string, Player>();

    public void Init()
    {
        var playerHub = HubConnectionManager.Instance.GetHubConnection(HubType.PlayerHub);
        playerHub.On<string, string>("UpdatePosition", OnUpdatePosition);
    }
    
    public void CreatePlayer(string clientId, string nickName)
    {
        if (_connectedPlayers.ContainsKey(clientId))
        {
            return;
        }
        
        MainThreadDispatcher.Instance.Enqueue(async () =>
        {
            var player = ServerManager.Instance.IsOwner(clientId) ? Instantiate(_ownerPlayerPrefab) : Instantiate(_playerPrefab);
            player.SetUp(nickName);

            var pos = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            player.SetAndBroadcastPosition(pos);
            
            _connectedPlayers.Add(clientId, player);
        });
    }

    public void DestroyPlayer(string clientId)
    {
        if (!_connectedPlayers.TryGetValue(clientId, out var player))
        {
            return;
        }
        
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            Destroy(player.gameObject);
            _connectedPlayers.Remove(clientId);
        });
        
    }
    
    public void OnUpdatePosition(string clientId, string position)
    {
        try
        {
            if (ServerManager.Instance.IsOwner(clientId))
            {
                return;
            }
            
            if (!_connectedPlayers.TryGetValue(clientId, out var player))
            {
                return;
            }

            var systemVector = JsonUtility.FromJson<System.Numerics.Vector3>(position);
            var unityVector = systemVector.ToUnityVector();

            player.SetPosition(unityVector);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }
}