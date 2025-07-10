using UnityEngine;

/// <summary>
/// 버스 앞문 , 뒷문 근처 트리거 영역을 관리, XR UI 버튼을 활성화.
/// 하차는 버스가 정류장에 도착해 대기 중일 때만(버튼 활성화) 가능.
/// </summary>
public class XRCanvasButtonTrigger : MonoBehaviour
{
    public enum ActionType
    {
        Board, // 탑승
        Exit // 하차
    }

    [Header("동작 종류 (탑승 / 하차)")]
    [SerializeField] private ActionType actionType;

    [Header("World Space Canvas UI")]
    [SerializeField] private GameObject UICanvas;

    [Header("버스 컨트롤러 (정류장 도착 여부 확인)")]
    [SerializeField] private BusController busController;

    [Header("탑승/하차 기능 처리 클래스")]
    [SerializeField] private BoardingManager boardingManager;

    private void Start()
    {
        if (UICanvas != null)
        {
            UICanvas.SetActive(false); // 시작 시 UI 버튼 비활성화
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거 영역에 들어오면 UI 버튼 활성화
        if (!other.CompareTag("Player")) return;

        // 하차 버튼은 버스가 대기 중일 때만 활성화
        if (actionType == ActionType.Exit && !busController.IsWaitingAtStop)
        {
            UICanvas.SetActive(false);
            return;
        }

        UICanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 트리거 영역을 벗어나면 UI 버튼 비활성화
            UICanvas.SetActive(false);
        }
    }

    /// <summary>
    /// XR UI 버튼 클릭 시 호출되는 메서드
    /// </summary>
    public void OnButtonCliked()
    {
        if (actionType == ActionType.Board)
        {
            boardingManager.BoardBus(); // 탑승
        }
        else
        {
            boardingManager.ExitBus(); // 하차
        }
    }
}
