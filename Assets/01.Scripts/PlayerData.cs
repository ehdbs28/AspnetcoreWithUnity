using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int id;
    public string name;
    public int score;
}

[System.Serializable]
public class PlayerDataWrapper
{
    public List<PlayerData> playerData = new List<PlayerData>();
}