public interface IGameHubConnector
{
    public void OnClientConnect(string clientId);
    public void OnPlayerJoined(string clientId, string nickName);
    public void OnPlayerLeft(string clientId);
}