using Backend.Server.ETC;
using Backend.Server.Object;

namespace Backend.Server.Core;

public class PlayerManager : Singleton<PlayerManager>
{
    private readonly ILogger<PlayerManager> _logger;
    private readonly Dictionary<string, Player> _players = new Dictionary<string, Player>();

    public PlayerManager()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });
        
        _logger = loggerFactory.CreateLogger<PlayerManager>();
    }
    
    public void ConnectPlayer(string clientId, string nickName)
    {
        if (_players.TryGetValue(clientId, out var player))
        {
            if (player.IsConnection)
            {
                _logger.LogWarning($"Already Connect Player: {clientId}");
            }
            else
            {
                player.Connect();     
                _logger.LogInformation($"Reconnect Player: {clientId}");
            }
            return;
        }

        var newPlayer = new Player();
        newPlayer.Connect(nickName);
        _players.Add(clientId, newPlayer);
    }

    public void DisconnectPlayer(string clientId)
    {
        if (!_players.TryGetValue(clientId, out var player))
        {
            _logger.LogWarning($"This Player Doesnt Connect To Server: {clientId}");
            return;
        }

        if (!player.IsConnection)
        {
            _logger.LogWarning($"This Player Already Disconnect: {clientId}");
            return;
        }

        player.DisConnect();
    }

    public Player? GetPlayer(string clientId)
    {
        return _players.GetValueOrDefault(clientId);
    }
}