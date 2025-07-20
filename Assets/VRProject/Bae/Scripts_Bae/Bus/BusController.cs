using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버스 정류장 경로 순회 및 도착 상태 관리 클래스.
/// 이동과 상태관리만 담당, UI나 탑승 처리는 다른 클래스에서 담당.
/// </summary>
public class BusController : MonoBehaviourPunCallbacks
{
    public event Action<float> OnStopStationEntered;
    public event Action OnStopStationExited;

    [System.Serializable]
    public class BusPathPoint
    {
        public Transform point; // 패스 포인트(이동 경로) 위치
        public bool isStopStation = false; // 해당 포인트가 정류장인지 여부 (True면 정류장, False면 경유지)
    }

    [Header("버스 이동 경로 위치 (순서대로 지정)")]
    [SerializeField] private List<BusPathPoint> pathPoints = new List<BusPathPoint>();

    [Header("버스 이동 관련 수치")]
    [SerializeField] private float busSpeed = 10f;
    [SerializeField] private float rotateSpeed = 2.5f;
    [SerializeField] private float pathWaitTime = 0.05f; // 경유지 대기 시간 (코너에서 자연스럽게 회전하기 위한 시간)
    [SerializeField] private float stopStationWaitTime = 20f; // 정류장에서 대기하는 시간

    private int previousPointIndex;
    private int nextPointIndex = 0;
    private float arrivalThreshold = 0.1f; // 버스가 목표 지점에 도착했다고 판단하는 거리

    private float syncedDepartureTime;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

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
            BusPathPoint nextPoint = pathPoints[nextPointIndex];

            // 목표 포인트까지 이동
            yield return MoveTowardsTarget(nextPoint.point);
            yield return HandleArrival(nextPoint);

            previousPointIndex = nextPointIndex;
            nextPointIndex = (nextPointIndex + 1) % pathPoints.Count;

            photonView.RPC("SetNextPointIndexRPC", RpcTarget.AllBuffered, nextPointIndex);
        }
    }
    private IEnumerator MoveTowardsTarget(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > arrivalThreshold)
        {
            // 이동
            transform.position = Vector3.MoveTowards(transform.position, target.position, busSpeed * Time.deltaTime);

            // 회전
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }

            yield return null;
        }

        transform.position = target.position; // 도착 위치 보정

        // 도착 방향을 이전 포인트 기준으로 보정
        Vector3 finalDirection = (target.position - pathPoints[previousPointIndex].point.position).normalized;
        finalDirection.y = 0f;
        if (finalDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(finalDirection);
        }

    }
    private IEnumerator HandleArrival(BusPathPoint point)
    {
        bool isStopStation = point.isStopStation;

        if (isStopStation)
            photonView.RPC("TriggerStopStationEntered", RpcTarget.All, stopStationWaitTime);


        float waitTime = isStopStation ? stopStationWaitTime : pathWaitTime;

        syncedDepartureTime = (float)PhotonNetwork.Time + waitTime;
        photonView.RPC("RPC_SetDepartureTime", RpcTarget.AllBuffered, syncedDepartureTime);

        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(waitTime);
        }
        else
        {
            while (PhotonNetwork.Time < syncedDepartureTime)
                yield return null;
        }

        if (isStopStation)
            photonView.RPC("TriggerStopStationExited", RpcTarget.All);
    }

    [PunRPC]
    private void TriggerStopStationEntered(float waitTime)
    {
        OnStopStationEntered?.Invoke(waitTime);
    }

    [PunRPC]
    private void TriggerStopStationExited()
    {
        OnStopStationExited?.Invoke();
    }
    [PunRPC]
    private void SetNextPointIndexRPC(int index)
    {
        nextPointIndex = index;
    }
    [PunRPC]
    private void RPC_SetDepartureTime(float serverTime)
    {
        syncedDepartureTime = serverTime;
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트 교체됨 - 버스 루틴 재시작");
            StopAllCoroutines();
            StartCoroutine(BusRoutineCo());
        }
    }

}