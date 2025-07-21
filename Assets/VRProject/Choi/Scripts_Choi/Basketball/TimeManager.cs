using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Time UI")]
    [SerializeField] private float timeLimit = 60f; // 1분
    [SerializeField] private Text timerText;
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text rewardText;

    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        currentTime = timeLimit;
        isRunning = true;
        gameOverUI.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;
            EndTimer();
        }

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void EndTimer()
    {
        gameOverUI.SetActive(true);
        // 점수 UI 갱신
        finalScoreText.text = " " + scoreManager.score;

        // 점수 기반 보상 지급
        int reward = scoreManager.score * 1000;
        CoinManager.Instance.AddCoins(reward);
        rewardText.text = $" + {reward}원";
        Debug.Log($"[TimeManager] {reward}원 보상 지급됨");
    }
}