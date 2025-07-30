using Firebase.Firestore;

[FirestoreData]
public class TopScoreData
{
    [FirestoreProperty]
    public string gameId { get; set; }

    [FirestoreProperty]
    public int score { get; set; }
}
