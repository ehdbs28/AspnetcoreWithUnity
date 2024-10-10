using System.Numerics;

namespace Backend.Server.Object;

public class Player
{
    public string NickName { get; private set; }
    public Vector3 Position { get; private set; }
    public bool IsConnection { get; private set; }

    public Player()
    {
        NickName = string.Empty;
        Position = Vector3.Zero;
        IsConnection = false;
    }

    public void Connect(string nickName)
    {
        NickName = nickName;
        IsConnection = true;
    }

    public void Connect()
    {
        IsConnection = true;
    }

    public void DisConnect()
    {
        IsConnection = false;
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        Position = newPosition;
    }
}