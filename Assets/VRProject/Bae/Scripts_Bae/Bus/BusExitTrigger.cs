using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// 버스 정류장 도착 시 하차 버튼 UI 활성화
/// </summary>
public class BusExitTrigger : MonoBehaviourPun
{
    [Header("버스 컨트롤러")]
    [SerializeField] private BusController busController; // 버스 컨트롤러 (정류장 대기 여부 판단용)

    [Header("버스 위치")]
    [SerializeField] private Transform busRoot; // 버스 본체 (부모로 붙일 대상)

    [Header("탑승 상태")]
    [SerializeField] private BoardingManager boardingManager; // 탑승 상태 관리 클래스

    // 플레이어 스폰 후에 수동 할당 -Choi
    private Transform player;
    private GameObject exitUICanvas; // 하차 버튼 UI

    private bool isPlayerInsideBus = false;
    private bool isBusAtStop = false;

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += OnPlayerSpawned;

        if (busController != null)
        {
            busController.OnStopStationEntered += HandleBusStopped;
            busController.OnStopStationExited += HandleBusDeparted;
        }

        if (boardingManager != null)
        {
            boardingManager.OnBoardedBus += HandleBoarded;
            boardingManager.OnExitedBus += HandleExited;
        }
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= OnPlayerSpawned;

        if (busController != null)
        {
            busController.OnStopStationEntered -= HandleBusStopped;
            busController.OnStopStationExited -= HandleBusDeparted;
        }

        if (boardingManager != null)
        {
            boardingManager.OnBoardedBus -= HandleBoarded;
            boardingManager.OnExitedBus -= HandleExited;
        }
    }

    private void OnPlayerSpawned(GameObject spawnedPlayer)
    {
        player = spawnedPlayer.transform;
        exitUICanvas = player.Find("UI/ExitCanvas")?.gameObject;

        if (exitUICanvas == null)
        {
            Debug.LogWarning("[BusExitTrigger] Exit UI를 찾을 수 없습니다");
            return;
        }

        exitUICanvas.SetActive(false);
    }

    private void HandleBusStopped(float waitTime)
    {
        isBusAtStop = true;
        UpdateUIVisibility();
    }

    private void HandleBusDeparted()
    {
        isBusAtStop = false;
        UpdateUIVisibility();
    }
    private void HandleBoarded()
    {
        isPlayerInsideBus = true;
        UpdateUIVisibility();
    }

    private void HandleExited()
    {
        isPlayerInsideBus = false;
        UpdateUIVisibility();
    }

    private void UpdateUIVisibility()
    {
        if (exitUICanvas == null) return;

        exitUICanvas.SetActive(isBusAtStop && isPlayerInsideBus);
    }
}
