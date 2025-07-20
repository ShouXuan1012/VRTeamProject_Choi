using Photon.Pun;
using UnityEngine;

/// <summary>
/// 트리거 진입 시 탑승 버튼 활성화
/// </summary>
public class XRCanvasButtonTrigger : MonoBehaviour
{
    [Header("탑승 UI")]
    [SerializeField] private GameObject UICanvas;

    [Header("버스 컨트롤러")]
    [SerializeField] private BusController busController;

    [Header("탑승/하차 기능 처리 클래스")]
    [SerializeField] private BoardingManager boardingManager;

    private string playerTag = "Player";

    private bool isPlayerInsideTrigger = false;
    private bool isBusWaitingAtStop = false;
    private bool isBoarded = false;

    private void Start()
    {
        if (UICanvas != null)
            UICanvas.SetActive(false);

        if (busController != null)
        {
            busController.OnStopStationEntered += HandleBusStopEntered;
            busController.OnStopStationExited += HandleBusStopExited;
        }

        if (boardingManager != null)
        {
            boardingManager.OnBoardedBus += HandleBoardedBus;
            boardingManager.OnExitedBus += HandleExitedBus;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag) || !IsLocalPlayer(other)) return;

        isPlayerInsideTrigger = true;
        UpdateUIVisibility();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag) || !IsLocalPlayer(other)) return;

        isPlayerInsideTrigger = false;
        UpdateUIVisibility();
    }

    private void HandleBusStopEntered(float waitTime)
    {
        isBusWaitingAtStop = true;
        UpdateUIVisibility();
    }
    private void HandleBusStopExited()
    {
        isBusWaitingAtStop = false;
        UpdateUIVisibility();
    }
    private void HandleBoardedBus()
    {
        isBoarded = true;
        UpdateUIVisibility();
    }
    private void HandleExitedBus()
    {
        isBoarded = false;
        UpdateUIVisibility();
    }

    private void UpdateUIVisibility()
    {
        if (UICanvas == null) return;

        UICanvas.SetActive(isPlayerInsideTrigger && isBusWaitingAtStop && !isBoarded);
    }

    public void OnButtonClicked()
    {
        boardingManager.BoardBus();
    }

    private bool IsLocalPlayer(Collider other)
    {
        PhotonView view = other.GetComponent<PhotonView>();
        return view != null && view.IsMine;
    }
}