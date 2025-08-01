using System.Collections.Generic;
using System.Threading.Tasks;

public class GameDataManager
{
    private static GameDataManager _instance;
    public static GameDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameDataManager();
            }
            return _instance;
        }
    }

    private FirestoreDBManager dbManager;

    private GameDataManager()
    {
        dbManager = FirestoreDBManager.Instance;
    }

    // 컬렉션 경로
    private const string collectionPath = "games";

    // 필드 경로
    private const string rankingPath = "ranking";

    public async Task<bool> SaveGameData(GameData gameData)
    {
        return await dbManager.TrySetDocumentAsync(collectionPath, gameData.gameId, gameData);
    }

    public async Task<bool> UpdateRanking(string gameId, List<RankingEntry> newRanking)
    {
        return await dbManager.TryUpdateFieldAsync(collectionPath, gameId, rankingPath, newRanking);
    }

    public async Task<GameData> GetGameData(string gameId)
    {
        return await dbManager.GetDocumentAsync<GameData>(collectionPath, gameId);
    }

    public async Task<List<GameData>> GetAllGames()
    {
        return await dbManager.GetCollectionAsync<GameData>(collectionPath);
    }
}
