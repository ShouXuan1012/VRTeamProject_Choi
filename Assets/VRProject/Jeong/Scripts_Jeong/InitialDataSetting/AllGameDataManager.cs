using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AllGameDataManager : MonoBehaviour
{
    public static AllGameDataManager Instance { get; private set; }
    public Dictionary<string, GameData> AllGameDatas { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadAllGameData();

        LocalHighScore.Instance.OnBestScoreUpdated += HandleBestScoreUpdated;
    }

    private async void LoadAllGameData()
    {
        List<GameData> AllGameDataList = await GameDataManager.Instance.GetAllGames();
        if (AllGameDataList == null || AllGameDataList.Count == 0)
        {
            Debug.LogWarning("게임 데이터가 없습니다.");
        }
        else
        {
            Debug.Log($"{AllGameDataList.Count}개의 게임 데이터를 로드했습니다.");

            AllGameDatas = new Dictionary<string, GameData>();
            foreach (GameData gameData in AllGameDataList)
            {
                if (gameData != null && !string.IsNullOrEmpty(gameData.gameId))
                {
                    AllGameDatas[gameData.gameId] = gameData;
                    Debug.Log($"게임 데이터 추가: {gameData.gameId} - {gameData.gameName}");
                }
                else
                {
                    Debug.LogWarning("게임 데이터가 유효하지 않습니다.");
                }
            }
        }
    }

    private async void HandleBestScoreUpdated(string gameId, int score)
    {
        GameData gameData = GetGameData(gameId);

        RankingEntry entry = new RankingEntry
        {
            userId = CurrentUserManager.Instance.CurrentUserData.userId,
            nickname = CurrentUserManager.Instance.CurrentUserData.nickname,
            score = score
        };

        gameData.ranking.Add(entry);
        gameData.ranking.Sort((a, b) => b.score.CompareTo(a.score));
        if (gameData.ranking.Count > 10)
        {
            gameData.ranking.RemoveRange(10, gameData.ranking.Count - 10);
        }

        SetRanking(gameData);
        await UpdateRanking(gameData);
    }

    private GameData GetGameData(string gameId)
    {
        if (AllGameDatas != null && AllGameDatas.TryGetValue(gameId, out GameData gameData))
        {
            return gameData;
        }
        else
        {
            // 데이터 없을 경우 빈 객체 리턴
            Debug.LogWarning($"게임 데이터가 없습니다: {gameId}");
            GameData newGameData = new GameData
            {
                gameId = gameId,
                gameName = "Unknown Game",
                ranking = new List<RankingEntry>()
            };
            return newGameData;
        }
    }

    private void SetRanking(GameData gameData)
    {
        if (AllGameDatas != null && AllGameDatas.ContainsKey(gameData.gameId))
        {
            AllGameDatas[gameData.gameId].ranking = gameData.ranking;
            Debug.Log($"랭킹 업데이트: {gameData.gameId}");
        }
        else
        {
            Debug.LogWarning($"게임 데이터가 없습니다: {gameData.gameId}");
        }
    }

    public async Task<GameData> LoadGameData(string gameId)
    {
        return await GameDataManager.Instance.GetGameData(gameId);
    }

    private async Task<bool> UpdateRanking(GameData gameData)
    {
        bool isUpdated = await GameDataManager.Instance.UpdateRanking(gameData.gameId, gameData.ranking);
        return isUpdated;
    }
}
