using System.Collections;
using UnityEngine;

/// <summary>
/// 버스 정류장 경로 순회 및 도착 상태 관리 클래스.
/// 이동과 상태관리만 담당, UI 나 탑승 처리는 다른 클래스에서 담당.
/// </summary>
public class BusController : MonoBehaviour
{
    [Header("정류장 경로 순서대로 지정")]
    [SerializeField] private Transform[] busStops; // 버스 정류장 위치 배열

    [Header("버스 속도 및 정류장 대기 시간")]
    [SerializeField] private float busSpeed = 5f; // 버스 이동 속도
    [SerializeField] private float waitTime = 10f; // 정류장 대기 시간

    private int currentStopIndex = 0; // 현재 정류장 인덱스

    public bool IsWaitingAtStop { get; private set; } // 현재 정류장에서 대기 중인지 여부

    private void Start()
    {
        if (busStops.Length > 0)
        {
            transform.position = busStops[0].position; // 초기 위치를 첫 번째 정류장으로 설정
            StartCoroutine(BusRoutineCo());
        }
    }

    /// <summary>
    /// 버스가 정류장을 순서대로 순회하며 도착 대기 상태를 관리하는 코루틴.
    /// </summary>
    private IEnumerator BusRoutineCo()
    {
        while (true)
        {
            Transform targetStop = busStops[currentStopIndex];

            // 다음 정류장으로 이동
            while (Vector3.Distance(transform.position, targetStop.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetStop.position, busSpeed * Time.deltaTime);
                yield return null; // 다음 프레임까지 대기
            }

            // 정류장에 도착
            transform.position = targetStop.position;
            IsWaitingAtStop = true; // 정류장 대기 상태로 전환

            yield return new WaitForSeconds(waitTime); // 대기 시간 동안 대기

            // 정류장 대기 상태 해제
            IsWaitingAtStop = false;
            currentStopIndex = (currentStopIndex + 1) % busStops.Length; // 다음 정류장으로 인덱스 이동
        }
    }
}
