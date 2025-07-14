using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 버스 정류장 경로 순회 및 도착 상태 관리 클래스.
/// 이동과 상태관리만 담당, UI 나 탑승 처리는 다른 클래스에서 담당.
/// </summary>


[System.Serializable] // 이 클래스를 인스펙터에서 볼 수 있도록 하기 위해 사용
public class BusPathPoint
{
    public Transform point; // 패스 포인트 위치
    public bool isStopStation = false; // 해당 포인트가 정류장인지 여부 (True면 정류장, False면 그냥 경유지)
}

public class BusController : MonoBehaviour
{
    [Header("버스 이동 경로 위치 (순서대로 지정)")]
    [SerializeField] private List<BusPathPoint> pathPoints = new List<BusPathPoint>();

    [Header("버스 이동 관련 수치")]
    [SerializeField] private float busSpeed = 10f; 
    [SerializeField] private float rotateSpeed = 2.5f; 
    [SerializeField] private float defaultWaitTime = 0.05f; // 패스 포인트 전환 대기 시간 (이동 중 대기 시간)
    [SerializeField] private float stopStationWaitTime = 7f; // 정류장에서 대기하는 시간

    public bool IsWaitingAtStop { get; private set; } // 현재 정류장에서 대기 중인지 여부

    private int currentPointIndex = 0;
    
    private void Start()
    {
        if (pathPoints.Count > 0)
        {
            // 처음 위치를 첫 번째 정류장으로 설정
            transform.position = pathPoints[0].point.position;
            // 이동 루틴 시작
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
            BusPathPoint current = pathPoints[currentPointIndex];
            Transform target = current.point;

            // 목표 포인트까지 이동
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                // 이동
                transform.position = Vector3.MoveTowards(transform.position, target.position, busSpeed * Time.deltaTime);

                // 회전
                Vector3 direction = (target.position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                }

                yield return null;
            }

            // 포인트에 도착
            transform.position = target.position;
            IsWaitingAtStop = true; // 현재 정류장에서 대기 중

            // "정류장" 이면 7초 대기, 그 외엔 0.05초 대기
            float waitTime = current.isStopStation ? stopStationWaitTime : defaultWaitTime;
            yield return new WaitForSeconds(waitTime);

            IsWaitingAtStop = false; // 현재 정류장에서 대기 중이 아님
            currentPointIndex = (currentPointIndex + 1) % pathPoints.Count;
        }
    }
}
