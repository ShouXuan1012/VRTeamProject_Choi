using UnityEngine;

public class AllGameDataManager : MonoBehaviour
{
    public static AllGameDataManager Instance { get; private set; }
    public GameData[] AllGameDatas { get; private set; }

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
        AllGameDatas = await GameDataManager.Instance.GetAllGames();
        if (AllGameDatas == null || AllGameDatas.Length == 0)
        {
            Debug.LogWarning("게임 데이터가 없습니다.");
        }
        else
        {
            Debug.Log($"{AllGameDatas.Length}개의 게임 데이터를 로드했습니다.");
        }
    }
}
