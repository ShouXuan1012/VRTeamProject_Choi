using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public class GameData
{
    [FirestoreProperty]
    public string gameId { get; set; }

    [FirestoreProperty]
    public string gameName { get; set; }

    [FirestoreProperty]
    public List<RankingEntry> ranking { get; set; }
}

[FirestoreData]
public class RankingEntry
{
    [FirestoreProperty]
    public string userId { get; set; }

    [FirestoreProperty]
    public string nickname { get; set; }

    [FirestoreProperty]
    public int score { get; set; }
}
