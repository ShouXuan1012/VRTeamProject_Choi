using System.Collections;
using UnityEngine;

public class XRCanvasButtonTrigger : MonoBehaviour
{
    public enum ActionType { Board, Exit }  // 버스 탑승/하차 기능을 위한 열거 enum

    [Header("동작 종류 (탑승 / 하차)")]
    [SerializeField] private ActionType actionType; // ActionType 열거형을 사용하여 탑승 또는 하차를 선택

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
            if (busController.IsStopStation)
                UICanvas.SetActive(true);
        }
        else
        {
            // 탑승은 조건 없이 활성화 (트리거 안에 들어오기만 하면)
            UICanvas.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (actionType == ActionType.Exit)
        {
            // 하차 버튼은 정류장 대기 중일 때만 활성화
            UICanvas.SetActive(busController != null
                && busController.IsStopStation  // 정류장에 대기 중인지 확인
                && busController.IsWaitingAtStop); // 버스가 대기 중인지 확인
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (UICanvas != null)
            UICanvas.SetActive(false);
    }

    public void OnButtonClicked()
    {
        if (actionType == ActionType.Board)
        {
            boardingManager.BoardBus();
            UICanvas.SetActive(false); // 버튼 클릭 후 UI 비활성화
        }
        else
        {
            boardingManager.ExitBus();
            UICanvas.SetActive(false); // 버튼 클릭 후 UI 비활성화
        }
    }
}