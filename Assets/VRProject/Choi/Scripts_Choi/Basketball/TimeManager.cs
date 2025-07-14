using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Time UI")]
    [SerializeField] private float timeLimit = 60f; // 1분
    [SerializeField] private Text timerText;

    [Header("GameOver UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText; // ScoreManager에서 점수 받아와서 표시할 Text
    [SerializeField] private ScoreManager scoreManager; // 점수 참조

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
        timerText.text = $" 시간 : {minutes:00}:{seconds:00}";
    }

    void EndTimer()
    {
        Debug.Log("타이머 끝! 게임 종료 처리");

        // 점수 UI 갱신
        scoreText.text = "최종 스코어 : " + scoreManager.score;

        // UI 활성화
        gameOverUI.SetActive(true);
    }
}
