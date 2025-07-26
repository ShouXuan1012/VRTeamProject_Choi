using Firebase.Firestore;

[FirestoreData]
public class UserData
{
    [FirestoreProperty]
    public string userId { get; set; }

    [FirestoreProperty]
    public string password { get; set; }

    [FirestoreProperty]
    public string nickname { get; set; }

    [FirestoreProperty]
    public string avatar { get; set; }

    [FirestoreProperty]
    public int coin { get; set; }
}
