using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Time UI")]
    [SerializeField] private float timeLimit = 60f; // 1분
    [SerializeField] private Text timerText;
    [SerializeField] private ScoreManager scoreManager;

    private GameObject gameOverUI;
    private Text scoreText;

    private float currentTime;
    private bool isRunning = false;

    void Start()
    {
        currentTime = timeLimit;
        isRunning = true;

        // 1. PlayerTest 오브젝트 찾기
        GameObject player = GameObject.Find("PlayerTest(Clone)");

        if (player != null)
        {

            // 2. GameoverUI 경로 따라 찾기
            Transform gameOverUITransform = player.transform.Find("Camera Offset/Main Camera/GameoverUI");

            if (gameOverUITransform != null)
            {
                gameOverUI = gameOverUITransform.gameObject;

                Transform scoreTextTransform = gameOverUITransform.Find("FinalScore");
                if (scoreTextTransform != null)
                {
                    scoreText = scoreTextTransform.GetComponent<Text>();
                }

                if (timerText == null)
                    timerText = GameObject.Find("TimerText")?.GetComponent<Text>();
            }

        }
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