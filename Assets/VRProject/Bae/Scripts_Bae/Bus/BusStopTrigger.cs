using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버스 정류장에 플레이어가 접근했을 때 하차 버튼 활성화
/// 버스 컨트롤러가 대기 중일 때만 활성화.
/// </summary>
public class BusStopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject exitButtonCanvas; // 하차 버튼 UI
    [SerializeField] private BusController busController; // 버스 컨트롤러 (정류장 대기 여부 판단용)

    void Start()
    {
        if (exitButtonCanvas != null)
            exitButtonCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // 버스가 정류장 대기 중일 때만 하차 버튼 활성화
        if (busController != null && busController.IsWaitingAtStop)
        {
            exitButtonCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 플레이어가 정류장 트리거를 벗어나면 하차 버튼 비활성화
        if (exitButtonCanvas != null)
            exitButtonCanvas.SetActive(false);
    }
}
