using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text rewardText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private ScoreManager scoreManager;

    public void HandleGameOver()
    {
        gameOverUI.SetActive(true);

        int score = scoreManager.score;
        finalScoreText.text = " " + score;

        int reward = score * 1000;
        CoinManager.Instance.AddCoins(reward);
        rewardText.text = $" + {reward}원";

        if (score >= 10)
        {
            Debug.Log("[퀘스트] 농구게임 퀘스트 완료 조건 달성");
            QuestEvents.Invoke(EQuestType.BasketballScored10);
        }

        // 최고 점수 갱신
        if (LocalHighScore.UpdateIfHigher(score))
        {
            Debug.Log($"[Local] 최고 점수 갱신됨: {score}");
            // 나중에 서버 업로드 여기에 추가하면 됨
        }
        else
        {
            Debug.Log($"[Local] 최고 점수 아님. 기존: {LocalHighScore.BestScore}");
        }

        bestScoreText.text = $" {LocalHighScore.BestScore}";
    }
}
