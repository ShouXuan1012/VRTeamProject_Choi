using UnityEngine;

public class XRCanvasButtonTrigger : MonoBehaviour
{
    public enum ActionType { Board, Exit }

    [Header("동작 종류 (탑승 / 하차)")]
    [SerializeField] private ActionType actionType;

    [Header("World Space Canvas UI")]
    [SerializeField] private GameObject UICanvas;

    [Header("버스 컨트롤러 (정류장 대기 여부 판단용)")]
    [SerializeField] private BusController busController;

    [Header("탑승/하차 기능 처리 클래스")]
    [SerializeField] private BoardingManager boardingManager;

    private void Start()
    {
        if (UICanvas != null)
            UICanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 하차 버튼은 정류장 대기 중일 때만 활성화
        if (actionType == ActionType.Exit)
        {
            if (busController != null && busController.IsWaitingAtStop)
            {
                UICanvas.SetActive(true);
            }
        }
        else
        {
            // 탑승은 조건 없이 활성화 (트리거 안에 들어오기만 하면)
            UICanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (UICanvas != null)
            UICanvas.SetActive(false);
    }

    public void OnButtonCliked()
    {
        if (actionType == ActionType.Board)
        {
            boardingManager.BoardBus();
        }
        else
        {
            boardingManager.ExitBus();
        }
    }
}