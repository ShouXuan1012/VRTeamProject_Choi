using System.Collections;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 플레이어의 버스 탑승 및 하차를 전담하는 클래스
/// 이동 제한 및 페이드 효과 등을 관리
/// </summary>
public class BoardingManager : MonoBehaviour
{
    [Header("좌석 위치(탑승 시 이동)")]
    [SerializeField] private Transform[] seatPositions;

    [Header("하차 위치")]
    [SerializeField] private Transform exitPosition;

    [Header("이동 제한 대상 컴포넌트")]
    [SerializeField] private GameObject locomotionProvider;

    private GameObject player;

    // 좌석 점유 상태 배열 (false : 비어 있음   , true : 점유 중)
    private bool[] seatOccupied;

    // 현재 앉아 있는 좌석 인덱스 (-1 : 아무 좌석도 앉아 있지 않음)
    private int currentSeatIndex = -1;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seatOccupied = new bool[seatPositions.Length];
    }

    /// <summary>
    /// 플레이어를 버스에 탑승시키는 메서드
    /// </summary>
    public void BoardBus()
    {
        StartCoroutine(BoardRoutineCo());
    }

    /// <summary>
    /// 플레이어를 버스에서 하차시키는 메서드
    /// </summary>
    
    public void ExitBus()
    {
        StartCoroutine(ExitRoutineCo());
    }

    private IEnumerator BoardRoutineCo()
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
        Debug.Log($"Player 이동 완료: {player.transform.position}");

        seatOccupied[seatIndex] = true;
        currentSeatIndex = seatIndex;

        if (controller != null) controller.enabled = true;

        if (locomotionProvider != null)
        {
            locomotionProvider.gameObject.SetActive(false);
        }

        yield return FadeUIController.Instance.FadeIn();
    }

    private IEnumerator ExitRoutineCo()
    {
        yield return FadeUIController.Instance.FadeOut();

        var controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        if (currentSeatIndex != -1)
        {
            seatOccupied[currentSeatIndex] = false; // 현재 좌석 비우기
            currentSeatIndex = -1; // 좌석 인덱스 초기화
        }

        if (controller != null) controller.enabled = true;

        if (locomotionProvider != null)
        {
            locomotionProvider.gameObject.SetActive(true);
        }

        yield return FadeUIController.Instance.FadeIn();
    }

    /// <summary>
    /// 비어 있는 좌석 인데스를 반환 (없으면 -1 반환)
    /// </summary>
    private int FindAvailableSeatIndex()
    {
        for (int i = 0; i < seatOccupied.Length; i++)
        {
            if (!seatOccupied[i])
            {
                return i; // 비어 있는 좌석 인덱스 반환
            }
        }
        return -1; // 모든 좌석이 점유 중인 경우
    }
}
