using UnityEngine;

/// <summary>
/// 버스 정류장 도착 시 하차 버튼 UI 활성화
/// </summary>
public class BusExitTrigger : MonoBehaviour
{
    [Header("버스 컨트롤러")]
    [SerializeField] private BusController busController; // 버스 컨트롤러 (정류장 대기 여부 판단용)

    [Header("버스 위치")]
    [SerializeField] private Transform busRoot; // 버스 본체 (부모로 붙일 대상)

    [Header("하차 버튼 UI")]
    [SerializeField] private GameObject exitUICanvas; // 하차 버튼 UI

    [Header("탑승 상태")]
    [SerializeField] private BoardingManager boardingManager; // 탑승 상태 관리 클래스

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 오브젝트 찾기
        if (exitUICanvas != null)
            exitUICanvas.SetActive(false); // 시작 시 하차 UI 비활성화
    }

    private void Update()
    {
        // 플레이어가 버스 안에 있는지 확인
        bool isBoarded = boardingManager != null && boardingManager.IsBoarded(); // 탑승 여부
        // 정류장 대기 상태
        bool isStop = busController != null && busController.IsStopStation && busController.IsWaitingAtStop;
        // 플레이어가 버스의 자식인지 확인
        bool isChildOfBus = player != null && player.IsChildOf(busRoot);    

        if (isBoarded && isChildOfBus && isStop)
        {
            if (!exitUICanvas.activeSelf)
                exitUICanvas.SetActive(true); // 하차 UI 활성화
        }
        else
        {
            if (exitUICanvas.activeSelf)
                exitUICanvas.SetActive(false); // 하차 UI 비활성화
        }

        //if (exitUICanvas.activeSelf 
        //    && busController != null 
        //    && !busController.IsStopStation)
        //{
        //    exitUICanvas.SetActive(false); // 정류장이 아닐 때 UI 비활성화
        //}
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag("Player")) return;

    //    isPlayerInside = true; // 플레이어가 트리거 안에 들어옴

    //    // 정류장일 때만 UI 활성화
    //    if (busController != null && busController.IsStopStation)
    //    {
    //        exitUICanvas.SetActive(true); // 하차 UI 활성화
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (!other.CompareTag("Player")) return;

    //    if (busController.IsStopStation && busController.IsWaitingAtStop)
    //    {
    //        // 정류장에 "대기 중일 때" 하차 UI 활성화
    //        if (!exitUICanvas.activeSelf)
    //            exitUICanvas.SetActive(true); // 하차 UI 활성화
    //    }
    //    else
    //    {
    //        // 정류장이 아니거나 "대기 중이 아닐 때" UI 비활성화
    //        if (exitUICanvas.activeSelf)
    //            exitUICanvas.SetActive(false); // 하차 UI 비활성화
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.CompareTag("Player")) return;
        
    //    isPlayerInside = false; // 플레이어가 트리거 밖으로 나감

    //    //if (exitUICanvas.activeSelf)
    //    //    exitUICanvas.SetActive(false); // 하차 UI 비활성화

    //    // 플레이어가 정류장을 떠날 때 UI 비활성화
    //    if (exitUICanvas != null)
    //    {
    //        exitUICanvas.SetActive(false); // 하차 UI 비활성화
    //    }
    //}

}
