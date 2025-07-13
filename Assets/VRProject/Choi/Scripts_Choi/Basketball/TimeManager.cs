using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    
    [SerializeField] private float timeLimit = 60f; // 1분
    [SerializeField] private Text timerText;

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
        Debug.Log("타이머 끝! 게임 종료 처리나 점수 집계 등 여기에 추가");
        // TODO: 타이머 종료 후 처리 (예: 게임 결과 보여주기, UI 전환 등)
    }
}
