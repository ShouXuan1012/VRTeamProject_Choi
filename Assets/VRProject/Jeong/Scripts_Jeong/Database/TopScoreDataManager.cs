using System.Threading.Tasks;

public class TopScoreDataManager
{
    private FirestoreDBManager dbManager = FirestoreDBManager.Instance;

    // 서브 컬렉션 경로
    private string GetUserScorePath(string userId)
    {
        return $"users/{userId}/top_scores";
    }

    // 필드 경로
    private const string scorePath = "score";

    public async Task<bool> SaveTopScoreData(string userId, string gameId, TopScoreData topScoreData)
    {
        return await dbManager.TrySetDocumentAsync(GetUserScorePath(userId), gameId, topScoreData);
    }

    public async Task<bool> UpdateScore(string userId, string gameId, int newScore)
    {
        return await dbManager.TryUpdateFieldAsync(GetUserScorePath(userId), gameId, scorePath, newScore);
    }

    public async Task<TopScoreData> GetUserScoreData(string userId, string gameId)
    {
        return await dbManager.GetDocumentAsync<TopScoreData>(GetUserScorePath(userId), gameId);
    }

    public async Task<TopScoreData[]> GetAllUserScores(string userId)
    {
        return await dbManager.GetCollectionAsync<TopScoreData>(GetUserScorePath(userId));
    }
}
