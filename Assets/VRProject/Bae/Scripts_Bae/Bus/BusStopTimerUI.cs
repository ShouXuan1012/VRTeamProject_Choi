using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BusStopTimerUI : MonoBehaviourPun
{
    //마찬가지로 수동 할당 - Choi
    private Text timerText;
    private Transform player;

    [Header("버스 컨트롤러")]
    [SerializeField] private BusController busController;

    private float currentWaitTime;

    private bool isCountingDown = false;

    private string baseText = "초 후 \n버스가 출발합니다!"; // UI에 표시할 기본 텍스트

    void Start()
    {
        StartCoroutine(SetupReferences());
    }
    
    private IEnumerator SetupReferences()
    {
        // 플레이어 기다리기
        while (PhotonNetwork.LocalPlayer == null || PhotonNetwork.LocalPlayer.TagObject == null)
            yield return null;

        player = (PhotonNetwork.LocalPlayer.TagObject as GameObject)?.transform;

        if (player == null)
        {
            Debug.LogError("[BusStopTimerUI] 플레이어를 찾을 수 없습니다.");
            yield break;
        }

        // 플레이어 자식에서 timerText 찾기
        timerText = player.Find("UI/ExitCanvas/B_E_Count_BackGround/E_CountDown_BackGround/E_Countdown_Text")?.GetComponent<Text>();

        if (timerText == null)
        {
            Debug.LogWarning("[BusStopTimerUI] TimerText를 찾을 수 없습니다.");
            yield break;
        }

        timerText.gameObject.SetActive(false); // 초기 비활성화
    }

    void Update()
    {
        if (timerText == null) return;

        // 정류장에 대기 중인 경우에만 타이머 활성화
        if (busController.IsStopStation && busController.IsWaitingAtStop)
        {
            if (!isCountingDown)
            {
                currentWaitTime = busController.stopStationWaitTime;
                timerText.gameObject.SetActive(true); // UI 활성화
                isCountingDown = true; // 카운트다운 시작
            }

            // 카운트다운 진행
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime < 0f) currentWaitTime = 0f; // 음수로 내려가지 않도록

            // UI 텍스트 업데이트
            int displayTime = Mathf.CeilToInt(currentWaitTime); // 소수점 올림
            timerText.text = displayTime + baseText;
        }
        else
        {
            // 대기 중이 아니면 UI 비활성화
            if (timerText != null && timerText.gameObject.activeSelf)
                timerText.gameObject.SetActive(false);

            isCountingDown = false; // 카운트다운 중지
        }
    }
}
