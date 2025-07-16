using System.Collections;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 플레이어의 버스 탑승 및 하차를 전담하는 클래스
/// 이동 제한 및 페이드 효과 등을 관리
/// </summary>
public class BoardingManager : MonoBehaviour
{
    [Header("UI 버튼(탑승/하차)")]
    [SerializeField] private GameObject boardUICanvas; // 탑승 버튼 UI
    [SerializeField] private GameObject exitUICanvas; // 하차 버튼 UI

    [Header("버스 상태 체크")]
    [SerializeField] private BusController busController; // 버스 컨트롤러 (정류장 대기 여부 판단용)

    [Header("이동 제한 대상 컴포넌트")]
    [SerializeField] private GameObject locomotionProvider;

    [Header("버스 본체(부모로 붙일 대상)")]
    [SerializeField] private Transform busRoot;

    [Header("좌석 위치(탑승 시 이동)")]
    [SerializeField] private Transform[] seatPositions;

    [Header("하차 위치")]
    [SerializeField] private Transform exitPosition;

    [SerializeField] private GameObject notEnoughMoneyUI; // 소지금 부족 UI
        
    private GameObject player;

    // 탑승 상태 배열 (false : 비어 있음, true : 탑승 중)
    private bool[] seatOccupied;

    private bool isBoarded = false; // 탑승 여부

    // 현재 앉아 있는 좌석 인덱스 (-1 : 아무 좌석도 앉아 있지 않음)
    private int currentSeatIndex = -1;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seatOccupied = new bool[seatPositions.Length];
    }

    // 탑승 상태에서만 하차 버튼 (UI) 활성화 상태 관리
    private void Update()
    {
        if (isBoarded)
        {
            // 탑승 중일 때 UI 상태 업데이트
            if (busController.IsWaitingAtStop)
            {
                // 정류장에 정차 중이면 하차 UI 활성화
                if (exitUICanvas != null && !exitUICanvas.activeSelf)
                {
                    exitUICanvas.SetActive(true);
                }
            }
            else
            {
                // 정류장에 정차 중이 아닐 때 하차 UI 비활성화
                if (exitUICanvas != null && exitUICanvas.activeSelf)
                {
                    exitUICanvas.SetActive(false);
                }
            }
        }
        else
        {
            // 플레이어가 탑승하지 않은 경우 하차 버튼 UI 비활성화
            if (exitUICanvas != null && exitUICanvas.activeSelf)
            {
                exitUICanvas.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 플레이어를 버스에 탑승시키는 메서드
    /// </summary>
    public void BoardBus()
    {
        // 아래 코드는 재화 UI 적용시킨 씬에서 정상작동 할거라 예상.
        // 현재는 탑승 버튼 상호작용 시 Null 오류가 나서 주석 처리함.
        //int boardingCost = 10000; // 탑승 비용

        //// 소지금 체크
        //if (!CoinManager.Instance.UseCoins(boardingCost))
        //{
        //    // 소지금 부족 시 안내 UI 표시
        //    if (notEnoughMoneyUI != null)
        //    {
        //        notEnoughMoneyUI.SetActive(true);
        //        StartCoroutine(HideNotEnoughMoneyUI());
        //    }
        //    return;
        //}

        StartCoroutine(BoardRoutineCo());
        isBoarded = true; // 탑승 상태로 변경
        HideAllUI();
    }

    /// <summary>
    /// 플레이어를 버스에서 하차시키는 메서드
    /// </summary>
    public void ExitBus()
    {
        StartCoroutine(ExitRoutineCo());
        isBoarded = false; // 탑승 상태 해제
        HideAllUI();
    }

    private IEnumerator BoardRoutineCo()    // 탑승 루틴
    {
        Debug.Log("탑승 루틴 시작");

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
        player.transform.position = seat.position;
        player.transform.rotation = seat.rotation;

        // 버스에 플레이어를 붙임 (버스가 움직이면 따라가게)
        if (busRoot != null)
        {
            player.transform.SetParent(busRoot);
        }

        seatOccupied[seatIndex] = true;
        currentSeatIndex = seatIndex;

        if (controller != null) controller.enabled = true;

        if (locomotionProvider != null)
        {
            locomotionProvider.gameObject.SetActive(false);
        }

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
        player.transform.rotation = exitPosition.rotation;

        if (currentSeatIndex != -1)
        {
            seatOccupied[currentSeatIndex] = false; // 좌석 비우기
            currentSeatIndex = -1;
        }

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

    public void ShowBoardUI()
    {
        // 탑승 버튼 UI 활성화
        if (!isBoarded && boardUICanvas != null)
        {
            boardUICanvas.SetActive(true);
        }
    }

    public void HideAllUI()
    {
        // 모든 UI 비활성화
        if (boardUICanvas != null) boardUICanvas.SetActive(false);
        if (exitUICanvas != null) exitUICanvas.SetActive(false);
    }
}
