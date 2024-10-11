using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public void SetUp(Character character)
    {
        
    }

    public void SetPosition(Vector3 newPosition)
    {
        MainThreadDispatcher.Instance.Enqueue(() =>
        {
            transform.position = newPosition;
        });
    }

    public void SetAndBroadcastPosition(Vector3 newPosition)
    {
        MainThreadDispatcher.Instance.Enqueue(async () =>
        {
            transform.position = newPosition;

            var playerHub = HubConnectionManager.Instance.GetHubConnection(HubType.PlayerHub);
            var jsonVector = JsonUtility.ToJson(newPosition.ToSystemVector());
            await playerHub.SendAsync("UpdatePlayerPosition", ServerManager.Instance.MyUserId, jsonVector);
        });
    }
}