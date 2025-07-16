using UnityEngine;

/// <summary>
/// 버스 정류장 도착 시 하차 버튼 UI 활성화
/// </summary>
public class BusExitTrigger : MonoBehaviour
{
    [Header("버스 상태 체크")]
    [SerializeField] private BusController busController; // 버스 컨트롤러 (정류장 대기 여부 판단용)

    [Header("하차 버튼 UI")]
    [SerializeField] private GameObject exitUICanvas; // 하차 버튼 UI

    void Start()
    {
        if (exitUICanvas != null)
            exitUICanvas.SetActive(false); // 시작 시 하차 UI 비활성화
    }

    private void Update()
    {
        if (exitUICanvas.activeSelf 
            && busController != null 
            && !busController.IsStopStation)
        {
            exitUICanvas.SetActive(false); // 정류장이 아닐 때 UI 비활성화
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 정류장일 때만 UI 활성화
        if (busController != null && busController.IsStopStation)
        {
            exitUICanvas.SetActive(true); // 하차 UI 활성화
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // 플레이어가 정류장을 떠날 때 UI 비활성화
        if (exitUICanvas != null)
        {
            exitUICanvas.SetActive(false); // 하차 UI 비활성화
        }
    }

}
