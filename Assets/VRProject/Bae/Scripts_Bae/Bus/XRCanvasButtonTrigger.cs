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
            boardingManager.OnBoardedBus += HideBoardUI;
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

    private void UpdateUIVisibility()
    {
        if (UICanvas == null) return;

        UICanvas.SetActive(isPlayerInsideTrigger && isBusWaitingAtStop);
    }
    private void HideBoardUI()
    {
        if (UICanvas == null) return;

        UICanvas.SetActive(false);
    }

    public void OnButtonClicked()
    {
        boardingManager.BoardBus();

        if (UICanvas != null)
            UICanvas.SetActive(false); // 버튼 클릭 후 UI 비활성화
    }

    private bool IsLocalPlayer(Collider other)
    {
        PhotonView view = other.GetComponent<PhotonView>();
        return view != null && view.IsMine;
    }
}