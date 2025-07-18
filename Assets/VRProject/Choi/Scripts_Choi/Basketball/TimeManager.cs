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

    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        currentTime = timeLimit;
        isRunning = true;       
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

        // 점수 UI 갱신
        finalScoreText.text = "최종 스코어 : " + scoreManager.score;

        // UI 활성화
        gameOverUI.SetActive(true);
    }


}