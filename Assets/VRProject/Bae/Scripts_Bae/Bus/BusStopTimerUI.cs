using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BusStopTimerUI : MonoBehaviourPun
{
    [Header("버스 컨트롤러")]
    [SerializeField] private BusController busController;

    [SerializeField] private Text busTimerText;

    // 플레이어 스폰 후에 수동 할당 -Choi
    private Transform player;
    private Text timerText;

    private Coroutine countdownRoutine;

    private string baseText = "초 후 \n버스가 출발합니다!"; // UI에 표시할 기본 텍스트

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += OnPlayerSpawned;

        if (busController != null)
        {
            busController.OnStopStationEntered += StartCountdown;
            busController.OnStopStationExited += HideCountdown;
        }
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= OnPlayerSpawned;

        if (busController != null)
        {
            busController.OnStopStationEntered -= StartCountdown;
            busController.OnStopStationExited -= HideCountdown;
        }
    }

    private void OnPlayerSpawned(GameObject spawnedPlayer)
    {
        player = spawnedPlayer.transform;
        timerText = player.Find("UI/ExitCanvas/B_E_Count_BackGround/E_CountDown_BackGround/E_Countdown_Text")?.GetComponent<Text>();

        if (timerText == null)
        {
            Debug.LogWarning("[BusStopTimerUI] TimerText를 찾을 수 없습니다.");
            return;
        }

        timerText.gameObject.SetActive(false); // 초기 비활성화
        busTimerText.gameObject.SetActive(false); // 초기 비활성화
    }

    private void StartCountdown(float waitTime)
    {
        if (timerText == null) return;
        timerText.gameObject.SetActive(true);

        if (busTimerText == null) return;
        busTimerText.gameObject.SetActive(true);

        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine = StartCoroutine(CountdownRoutine(waitTime));
    }

    private IEnumerator CountdownRoutine(float waitTime)
    {
        float timeLeft = waitTime;

        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            int displayTime = Mathf.CeilToInt(timeLeft);
            timerText.text = displayTime + baseText;
            busTimerText.text = displayTime + baseText;
            yield return null;
        }
    }

    private void HideCountdown()
    {
        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
            countdownRoutine = null;
        }

        if (timerText != null)
            timerText.gameObject.SetActive(false);

        if (busTimerText != null)
            busTimerText.gameObject.SetActive(false);
    }
}
