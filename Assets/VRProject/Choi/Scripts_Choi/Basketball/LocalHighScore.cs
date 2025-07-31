using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LocalHighScore : MonoBehaviour
{
    public static LocalHighScore Instance { get; private set; }

    private Dictionary<string, TopScoreData> topScoreDict;

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

    private void Start()
    {
        topScoreDict = CurrentUserManager.Instance.TopScoreDict ?? new Dictionary<string, TopScoreData>();
    }

    public int GetBestScore(string gameId)
    {
        if (topScoreDict.TryGetValue(gameId, out TopScoreData data))
        {
            return data.score;
        }
        return 0;
    }

    public async Task<bool> UpdateIfHigher(string gameId, int score)
    {
        int bestScore = 0;
        if (topScoreDict.TryGetValue(gameId, out TopScoreData existingData))
        {
            bestScore = existingData.score;
        }

        if (score > bestScore)
        {
            bestScore = score;

            TopScoreData data = new TopScoreData
            {
                gameId = gameId,
                score = bestScore
            };

            topScoreDict[gameId] = data;
            CurrentUserManager.Instance.SetTopScore(data);

            bool isSaved = await CurrentUserManager.Instance.AddOrUpdateTopScoreData(data);
            if (!isSaved)
            {
                Debug.LogError($"최고 점수 저장에 실패했습니다.");
                return false;
            }

            return true;
        }
        return false;
    }
}
