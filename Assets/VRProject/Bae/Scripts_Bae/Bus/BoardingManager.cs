using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

/// <summary>
/// 플레이어의 버스 탑승 및 하차를 전담하는 클래스
/// 이동 제한 및 페이드 효과 등을 관리
/// </summary>
public class BoardingManager : MonoBehaviourPun
{
    public event Action OnBoardedBus;
    public event Action OnExitedBus;

    [Header("UI 버튼 (탑승)")]
    [SerializeField] private GameObject boardUICanvas; // 탑승 버튼 UI

    [Header("버스 컨트롤러")]
    [SerializeField] private BusController busController; // 버스 컨트롤러 (정류장 대기 여부 판단용)

    [Header("좌석 위치(탑승 시 이동)")]
    [SerializeField] private Transform[] seatPositions;

    [Header("탑승 시 바라볼 방향 (운전사)")]
    [SerializeField] private Transform lookTarget;

    [Header("하차 위치 및 바라볼 방향")]
    [SerializeField] private Transform exitPosition;
    [SerializeField] private Transform exitLookTarget;

    [SerializeField] private GameObject notEnoughMoneyUI; // 소지금 부족 UI   

    private GameObject player;
    private Transform mainCamera; // 메인 카메라 (탑승 시 바라볼 방향 설정용)
    private GameObject locomotionProvider; //이동 제한 대상 컴포넌트

    private bool[] seatOccupied; // 탑승 상태 배열 (false : 비어 있음, true : 탑승 중)
    private int currentSeatIndex = -1; // 현재 앉아 있는 좌석 인덱스 (-1 : 아무 좌석도 앉아 있지 않음)

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += SetupReferences;
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= SetupReferences;
    }

    private void Awake()
    {
        seatOccupied = new bool[seatPositions.Length];
    }

    private void SetupReferences(GameObject spawnedPlayer)
    {
        player = spawnedPlayer;

        Transform cam = player.transform.Find("Camera Offset/Main Camera");
        if (cam != null) mainCamera = cam;
        else Debug.LogWarning("MainCamera 찾기 실패");

        Transform loco = player.transform.Find("Locomotion System");
        if (loco != null) locomotionProvider = loco.gameObject;
        else Debug.LogWarning("LocomotionProvider 찾기 실패");

        Transform buttonTr = player.transform.Find("UI/ExitCanvas/E_Button");
        if (buttonTr != null)
        {
            var button = buttonTr.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ExitBus);
        }
        else
        {
            Debug.LogWarning("ExitButton 찾기 실패");
        }
    }

    /// <summary>
    /// 플레이어를 버스에 탑승시키는 메서드
    /// </summary>
    public void BoardBus()
    {
        // 아래 코드는 재화 UI 적용시킨 씬에서 정상작동 할거라 예상.
        // 현재는 탑승 버튼 상호작용 시 Null 오류가 나서 주석 처리함.
        int boardingCost = 10000; // 탑승 비용

        // 소지금 체크
        if (!CoinManager.Instance.UseCoins(boardingCost))
        {
            // 소지금 부족 시 안내 UI 표시
            if (notEnoughMoneyUI != null)
            {
                notEnoughMoneyUI.SetActive(true);
                StartCoroutine(HideNotEnoughMoneyUI());
            }
            return;
        }

        StartCoroutine(BoardRoutineCo());
    }

    /// <summary>
    /// 플레이어를 버스에서 하차시키는 메서드
    /// </summary>
    public void ExitBus()
    {
        StartCoroutine(ExitRoutineCo());
        OnExitedBus?.Invoke();
    }

    private IEnumerator BoardRoutineCo()    // 탑승 루틴
    {
        yield return FadeUIController.Instance.FadeOut();

        if (player == null)
        {
            Debug.LogError("player를 찾을 수 없습니다.");
            yield break;
        }

        var controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        int seatIndex = FindAvailableSeatIndex();
        if (seatIndex == -1)
        {
            Debug.LogWarning("탑승 가능한 좌석이 없습니다.");
            yield return FadeUIController.Instance.FadeIn();
            yield break;
        }

        Transform seat = seatPositions[seatIndex];

        // 플레이어를 좌석 위치로 이동
        player.transform.position = seat.position;

        // 플레이어가 바라볼 방향 설정
        if (lookTarget != null)
        {
            Vector3 dir = (lookTarget.position - player.transform.position).normalized;
            dir.y = 0f; // 수평 방향으로만 바라보기
            if (dir != Vector3.zero)
                player.transform.forward = dir; // 플레이어가 운전사 방향으로 바라보게 설정
            if (mainCamera != null) mainCamera.transform.forward = dir;
        }
        else
        {
            // 좌석 방향으로 바라보기
            player.transform.rotation = seat.rotation;
        }

        // 버스에 플레이어를 붙임 (버스가 움직이면 따라가게)
        if (this.transform != null)
        {
            player.transform.SetParent(this.transform);
        }

        photonView.RPC("SetSeatOccupiedRPC", RpcTarget.AllBuffered, seatIndex, true);
        currentSeatIndex = seatIndex;

        RemoteTransformSync remoteTransformSync = player.GetComponent<RemoteTransformSync>();
        if (remoteTransformSync != null)
        {
            remoteTransformSync.enabled = false; // 탑승 시 동기화 비활성화
        }
        photonView.RPC("AssignSeatPosition", RpcTarget.OthersBuffered, seatIndex, player.GetComponent<PhotonView>().ViewID); // 좌석 위치 동기화

        if (controller != null) controller.enabled = true;

        if (locomotionProvider != null)
        {
            locomotionProvider.gameObject.SetActive(false);
        }

        OnBoardedBus?.Invoke();
        QuestEvents.Invoke(EQuestType.BusBoarded);
        AchievementManager.Instance.AddProgress(EAchievementType.BusRider, 1);
        yield return FadeUIController.Instance.FadeIn();
    }

    private IEnumerator ExitRoutineCo()     // 하차 루틴
    {
        yield return FadeUIController.Instance.FadeOut();

        var controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        // 버스에서 플레이어 떼어내기
        player.transform.SetParent(null);

        player.transform.position = exitPosition.position;
        // 플레이어가 바라볼 방향 설정
        if (exitLookTarget != null)
        {
            Vector3 dir = (exitLookTarget.position - player.transform.position).normalized;
            dir.y = 0f; // 수평 방향으로만 바라보기
            if (dir != Vector3.zero)
                player.transform.forward = dir; // 플레이어가 하차 후 바라볼 방향 설정
        }

        if (currentSeatIndex != -1)
        {
            photonView.RPC("SetSeatOccupiedRPC", RpcTarget.AllBuffered, currentSeatIndex, false);
            currentSeatIndex = -1;
        }

        RemoteTransformSync remoteTransformSync = player.GetComponent<RemoteTransformSync>();
        if (remoteTransformSync != null)
        {
            remoteTransformSync.enabled = true; // 하차 시 다시 활성화
        }
        photonView.RPC("ClearSeatPosition", RpcTarget.OthersBuffered, player.GetComponent<PhotonView>().ViewID); // 좌석 위치 동기화 해제

        if (controller != null) controller.enabled = true;

        if (locomotionProvider != null)
        {
            locomotionProvider.gameObject.SetActive(true);
        }

        yield return FadeUIController.Instance.FadeIn();
    }

    private IEnumerator HideNotEnoughMoneyUI()
    {
        yield return new WaitForSeconds(2f); // 2초 후에 UI 숨김
        if (notEnoughMoneyUI != null)
            notEnoughMoneyUI.SetActive(false);
    }

    /// <summary>
    /// 비어 있는 좌석 인덱스를 반환 (없으면 -1)
    /// </summary>
    private int FindAvailableSeatIndex()
    {
        for (int i = 0; i < seatOccupied.Length; i++)
        {
            if (!seatOccupied[i])
            {
                return i;
            }
        }
        return -1;
    }

    [PunRPC]
    private void SetSeatOccupiedRPC(int seatIndex, bool isOccupied)
    {
        if (seatIndex >= 0 && seatIndex < seatOccupied.Length)
        {
            seatOccupied[seatIndex] = isOccupied;
        }
    }
    [PunRPC]
    void AssignSeatPosition(int index, int viewID)
    {
        PhotonView view = PhotonView.Find(viewID);
        if (view == null) return;

        var follower = view.GetComponent<SeatSyncFollower>();
        if (follower != null)
        {
            follower.Initialize(transform, seatPositions);
            follower.AssignSeat(index);
        }

        RemoteTransformSync remoteTransformSync = view.GetComponent<RemoteTransformSync>();
        if (remoteTransformSync != null)
        {
            remoteTransformSync.enabled = false; // 탑승 시 동기화 비활성화
        }
    }
    [PunRPC]
    void ClearSeatPosition(int viewID)
    {
        PhotonView view = PhotonView.Find(viewID);
        if (view == null) return;

        var follower = view.GetComponent<SeatSyncFollower>();
        if (follower != null)
        {
            follower.ClearSeat();
        }

        RemoteTransformSync remoteTransformSync = view.GetComponent<RemoteTransformSync>();
        if (remoteTransformSync != null)
        {
            remoteTransformSync.enabled = true;
        }
    }
}
