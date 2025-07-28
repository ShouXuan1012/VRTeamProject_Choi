using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text rewardText;
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
            QuestEvents.BasketballScored10();
        }

        //여기다가 이제 최고 점수 매니저에 저장
    }
}
