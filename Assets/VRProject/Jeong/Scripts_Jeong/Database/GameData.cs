using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public string gameId;
    public string gameName;
    public List<RankingEntry> ranking;
}

[System.Serializable]
public class RankingEntry
{
    public string userId;
    public string nickname;
    public int score;
}
