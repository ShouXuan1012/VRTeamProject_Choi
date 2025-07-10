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
    [SerializeField] private Transform seatPosition;

    [Header("하차 위치")]
    [SerializeField] private Transform exitPosition;

    [Header("이동 제한 대상 컴포넌트")]
    [SerializeField] private Behaviour locomotionProvider;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            Debug.LogError("player가 null입니다.");
            yield break;
        }

        var controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.transform.position = seatPosition.position;
        player.transform.rotation = seatPosition.rotation;
        Debug.Log($"XR Origin 이동 완료: {player.transform.position}");

        if (controller != null) controller.enabled = true;

        if (locomotionProvider != null)
        {
            locomotionProvider.enabled = false;
        }

        yield return FadeUIController.Instance.FadeIn();
    }

    private IEnumerator ExitRoutineCo()
    {
        yield return FadeUIController.Instance.FadeOut();

        player.transform.position = exitPosition.position;
        player.transform.rotation = exitPosition.rotation;

        if (locomotionProvider != null)
        {
            locomotionProvider.enabled = true; // 이동 제한 해제
        }

        yield return FadeUIController.Instance.FadeIn();
    }

}
