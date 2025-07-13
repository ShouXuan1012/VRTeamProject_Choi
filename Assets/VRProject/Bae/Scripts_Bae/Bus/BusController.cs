using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 버스 정류장 경로 순회 및 도착 상태 관리 클래스.
/// 이동과 상태관리만 담당, UI 나 탑승 처리는 다른 클래스에서 담당.
/// </summary>
public class BusController : MonoBehaviour
{
    [Header("정류장 위치 (순서대로 지정)")]
    [SerializeField] private Transform[] pathPoints;

    [Header("버스 이동 속도 / 회전 속도 / 정류장 대기 시간")]
    [SerializeField] private float busSpeed = 5f; 
    [SerializeField] private float rotateSpeed = 5f; 
    [SerializeField] private float waitTime = 10f; 

    public bool IsWaitingAtStop { get; private set; } // 현재 정류장에서 대기 중인지 여부

    private int currentPointIndex = 0;
    
    private void Start()
    {
        if (pathPoints.Length > 0)
        {
            // 처음 위치를 첫 번째 정류장으로 설정
            transform.position = pathPoints[0].position;
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
            Transform target = pathPoints[currentPointIndex];

            // 목표 포인트까지 이동
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                // 이동
                transform.position = Vector3.MoveTowards(transform.position, target.position, busSpeed * Time.deltaTime);

                // 회전 (버스가 이동 방향을 향하도록)
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
            IsWaitingAtStop = true;

            yield return new WaitForSeconds(waitTime);

            IsWaitingAtStop = false;
            currentPointIndex = (currentPointIndex + 1) % pathPoints.Length;
        }
    }
}
