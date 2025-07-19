using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// 트리거 진입 시 탑승 버튼만 활성화, 하차 UI는 정류장 대기 중일 때만 활성화.
/// </summary>
public class XRCanvasButtonTrigger : MonoBehaviour
{
    public enum ActionType { Board, Exit }  // 버스 탑승/하차 기능을 위한 열거 enum

    [Header("동작 종류 (탑승 / 하차)")]
    [SerializeField] private ActionType actionType; // ActionType 열거형을 사용하여 탑승 또는 하차를 선택

    [Header("탑승 UI")]
    [SerializeField] private GameObject UICanvas;

    [Header("버스 컨트롤러")]
    [SerializeField] private BusController busController;

    [Header("탑승/하차 기능 처리 클래스")]
    [SerializeField] private BoardingManager boardingManager;

    private string playerTag = "Player";

    private void Start()
    {
        if (UICanvas != null)
            UICanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (!other.CompareTag(playerTag)) return;
        if (other.CompareTag("Player") && IsLocalPlayer(other))
        {
            Debug.Log("탑승 트리거 입장");
            // 트리거에 플레이어가 들어오면 UI 활성화
            Debug.Log($"actionType: {actionType}, UICanvas is null: {UICanvas == null}, busController.IsStopStation: {busController.IsStopStation}");
            if (actionType == ActionType.Board && UICanvas != null && busController.IsStopStation)
                UICanvas.SetActive(true);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (!other.CompareTag("Player")) return;

    //    if (actionType == ActionType.Exit)
    //    {
    //        // 하차 버튼은 정류장 대기 중일 때만 활성화
    //        UICanvas.SetActive(busController != null
    //            && busController.IsStopStation  // 정류장에 대기 중인지 확인
    //            && busController.IsWaitingAtStop); // 버스가 대기 중인지 확인
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (actionType == ActionType.Board && UICanvas != null)
            UICanvas.SetActive(false);
    }

    public void OnButtonClicked()
    {
        if (actionType == ActionType.Board)
        {
            boardingManager.BoardBus();
        }
        else if (actionType == ActionType.Exit)
        {
            boardingManager.ExitBus();
        }

        if (UICanvas != null)
            UICanvas.SetActive(false); // 버튼 클릭 후 UI 비활성화
    }

    private bool IsLocalPlayer(Collider other)
    {
        PhotonView view = other.GetComponent<PhotonView>();
        return view != null && view.IsMine;
    }
}