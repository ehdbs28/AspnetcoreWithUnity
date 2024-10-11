using System.Collections.Generic;
using System.Numerics;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>, IPlayerHubConnector
{
    [SerializeField] private MainPlayer _ownerPlayerPrefab;
    [SerializeField] private Player _playerPrefab;

    private readonly Dictionary<int, Player> _connectedPlayers = new Dictionary<int, Player>();
    
    public void Init()
    {
        var playerHub = HubConnectionManager.Instance.GetHubConnection(HubType.PlayerHub);

        playerHub.On<string>("CreatePlayer", OnCreatePlayer);
        playerHub.On<int>("DeletePlayer", OnDeletePlayer);
        playerHub.On<int, string>("UpdatePosition", OnUpdatePosition);
    }
    
    public async void CreatePlayer(int userId)
    {
        var playerHub = HubConnectionManager.Instance.GetHubConnection(HubType.PlayerHub);
        await playerHub.SendAsync("CreatePlayer", userId);
    }

    public void DeletePlayer(int userId)
    {
        MainThreadDispatcher.Instance.Enqueue(async () =>
        {
            if (!_connectedPlayers.TryGetValue(userId, out var player))
            {
                return;
            }
            
            var jsonData = JsonUtility.ToJson(player.GetData());

            var playerHub = HubConnectionManager.Instance.GetHubConnection(HubType.PlayerHub);
            await playerHub.SendAsync("DeletePlayer", jsonData);
        });
    }

    public void OnCreatePlayer(string characterData)
    {
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            var character = JsonUtility.FromJson<Character>(characterData);
            
            var player = ServerManager.Instance.IsOwner(character.OwnerUserId) ? Instantiate(_ownerPlayerPrefab) : Instantiate(_playerPrefab);
            player.SetUp(character);

            Debug.Log(ServerManager.Instance.IsOwner(character.OwnerUserId));
            
            _connectedPlayers.Add(character.OwnerUserId, player);
        });
    }

    public void OnDeletePlayer(int userId)
    {
        if (!_connectedPlayers.Remove(userId, out var player))
        {
            return;
        }
        
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            Destroy(player.gameObject);

            if (ServerManager.Instance.IsOwner(userId))
            {
                ServerManager.Instance.IsLogOut = true;
            }
        });
    }

    public void OnUpdatePosition(int userId, string jsonPosition)
    {
        if (ServerManager.Instance.IsOwner(userId))
        {
            return;
        }
        
        if (!_connectedPlayers.TryGetValue(userId, out var player))
        {
            return;
        }
        
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            var position = JsonUtility.FromJson<System.Numerics.Vector3>(jsonPosition);
            player.SetPosition(position.ToUnityVector());
        });
    }
}