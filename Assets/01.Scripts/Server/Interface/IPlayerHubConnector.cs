
using System.Numerics;

public interface IPlayerHubConnector
{
    public void OnUpdatePosition(string clientId, string newPositionJson);
}