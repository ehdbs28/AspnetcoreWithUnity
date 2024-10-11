using System;

public class GameManager : MonoSingleton<GameManager>
{
    public event Action OnSetEvent = null;
    
    public bool IsSet { get; private set; }
    
    private async void Start()
    {
        IsSet = false;
        await HubConnectionManager.Instance.Init();
        ServerManager.Instance.Init();
        PlayerManager.Instance.Init();

        await HubConnectionManager.Instance.ConnectToServer();
        IsSet = true;
        OnSetEvent?.Invoke();
    }
}