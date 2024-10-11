using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public async void DeletePlayer(int userId)
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
            
            _connectedPlayers.Add(character.OwnerUserId, player);
        });
    }

    public void OnDeletePlayer(int userId)
    {
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            if (!_connectedPlayers.Remove(userId, out var player))
            {
                return;
            }

            Destroy(player.gameObject);

            if (ServerManager.Instance.IsOwner(userId))
            {
                ServerManager.Instance.IsLogOut = true;
            }
        });
    }

    public void OnUpdatePosition(int userId, string jsonPosition)
    {
        MainThreadDispatcher.Instance.Enqueue(async () =>
        {
            if (!_connectedPlayers.TryGetValue(userId, out var player))
            {
                return;
            }
            
            var position = JsonUtility.FromJson<Vector3>(jsonPosition);
            player.SetPosition(position);
        });
    }
}