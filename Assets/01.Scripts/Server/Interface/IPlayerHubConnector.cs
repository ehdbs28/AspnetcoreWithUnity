
using System.Numerics;

public interface IPlayerHubConnector
{
    public void OnCreatePlayer(Character character);
    public void OnDeletePlayer(int userId);
    public void OnUpdatePosition(int userId, string jsonPosition);
}