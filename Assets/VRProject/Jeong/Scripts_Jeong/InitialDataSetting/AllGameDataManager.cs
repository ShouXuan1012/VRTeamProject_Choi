using System.Collections.Generic;
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
    }

    public async void LoadAllGameData()
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
}
