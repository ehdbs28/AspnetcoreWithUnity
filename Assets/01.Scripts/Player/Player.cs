using System;
using Microsoft.AspNetCore.SignalR.Client;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nickNameText;
    
    private int _level;

    protected virtual void Awake()
    {
        var canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public async void SetUp(Character character)
    {
        var gameHub = HubConnectionManager.Instance.GetHubConnection(HubType.GameHub);
        var userName = await gameHub.InvokeAsync<string>("GetUserName", character.OwnerUserId);
        
        _nickNameText.text = userName;
        
        _level = character.Level;
        transform.position = new Vector3
        (
            character.LastPositionX,
            character.LastPositionY,
            character.LastPositionZ
        );
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

    public Character GetData()
    {
        return new Character
        {
            OwnerUserId = ServerManager.Instance.MyUserId,
            Level = _level,
            LastPositionX = transform.position.x,
            LastPositionY = transform.position.y,
            LastPositionZ = transform.position.z
        };
    }
}