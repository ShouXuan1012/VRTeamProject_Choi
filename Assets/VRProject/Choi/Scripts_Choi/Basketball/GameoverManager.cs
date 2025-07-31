using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Configuration")]
    [SerializeField] private string gameId = "basketball";

    [Header("Game Over UI Elements")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text rewardText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private ScoreManager scoreManager;

    public async void HandleGameOver()
    {
        gameOverUI.SetActive(true);

        int score = scoreManager.score;
        finalScoreText.text = " " + score;

        int reward = score * 1000;
        CoinManager.Instance.AddCoins(reward);
        rewardText.text = $" + {reward}원";

        if(gameId == "basketball")
        {
            if (score >= 10)
            {
                Debug.Log("[퀘스트] 농구게임 퀘스트 완료 조건 달성");
                QuestEvents.Invoke(EQuestType.BasketballScored10);
            }
        }

        // 최고 점수 갱신
        await LocalHighScore.Instance.UpdateIfHigher(gameId, score);

        bestScoreText.text = $" {LocalHighScore.Instance.GetBestScore(gameId)}";
    }
}
