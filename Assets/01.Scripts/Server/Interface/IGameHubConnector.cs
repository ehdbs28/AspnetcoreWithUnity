public interface IGameHubConnector
{
    public void OnLoginFailure();
    public void OnLoginSuccess(int userId);
    public void OnLogOut(int userId);
}