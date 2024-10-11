using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : MonoSingleton<PlayerManager>, IPlayerHubConnector
{
    [SerializeField] private MainPlayer _ownerPlayerPrefab;
    [SerializeField] private Player _playerPrefab;
    
    public void Init()
    {
        var playerHub = HubConnectionManager.Instance.GetHubConnection(HubType.PlayerHub);

        playerHub.On<Character>("CreatePlayer", OnCreatePlayer);
        playerHub.On<int>("DeletePlayer", OnDeletePlayer);
        playerHub.On<int, string>("UpdatePosition", OnUpdatePosition);
    }
    
    public async Task CreatePlayer(int userId)
    {
        var playerHub = HubConnectionManager.Instance.GetHubConnection(HubType.PlayerHub);
        await playerHub.SendAsync("CreatePlayer", userId);
    }

    public async Task DeletePlayer(int userId)
    {
        var playerHub = HubConnectionManager.Instance.GetHubConnection(HubType.PlayerHub);
        await playerHub.SendAsync("DeletePlayer", userId);
        // if (!_connectedPlayers.TryGetValue(clientId, out var player))
        // {
        //     return;
        // }
        //
        // MainThreadDispatcher.Instance.Enqueue(() =>
        // {
        //     Destroy(player.gameObject);
        //     _connectedPlayers.Remove(clientId);
        // });
    }
    
    public void UpdatePosition(int userId, Vector3 position)
    {
        // try
        // {
        //     if (ServerManager.Instance.IsOwner(clientId))
        //     {
        //         return;
        //     }
        //     
        //     if (!_connectedPlayers.TryGetValue(clientId, out var player))
        //     {
        //         return;
        //     }
        //
        //     var systemVector = JsonUtility.FromJson<System.Numerics.Vector3>(position);
        //     var unityVector = systemVector.ToUnityVector();
        //
        //     player.SetPosition(unityVector);
        // }
        // catch (Exception e)
        // {
        //     Debug.LogError(e);
        //     throw;
        // }
    }

    public void OnCreatePlayer(Character character)
    {
        MainThreadDispatcher.Instance.Enqueue(async () =>
        {
            var player = ServerManager.Instance.IsOwner(character.OwnerUserId) ? Instantiate(_ownerPlayerPrefab) : Instantiate(_playerPrefab);
            player.SetUp(character);

            var pos = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            player.SetAndBroadcastPosition(pos);
        });
    }

    public void OnDeletePlayer(int userId)
    {
    }

    public void OnUpdatePosition(int userId, string jsonPosition)
    {
    }
}