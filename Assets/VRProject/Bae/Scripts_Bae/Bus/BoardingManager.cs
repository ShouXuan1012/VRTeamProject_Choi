using System.Collections;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// �÷��̾��� ���� ž�� �� ������ �����ϴ� Ŭ����
/// �̵� ���� �� ���̵� ȿ�� ���� ����
/// </summary>
public class BoardingManager : MonoBehaviour
{
    [Header("�¼� ��ġ(ž�� �� �̵�)")]
    [SerializeField] private Transform[] seatPositions;

    [Header("���� ��ġ")]
    [SerializeField] private Transform exitPosition;

    [Header("�̵� ���� ��� ������Ʈ")]
    [SerializeField] private GameObject locomotionProvider;

    private GameObject player;

    // �¼� ���� ���� �迭 (false : ��� ����   , true : ���� ��)
    private bool[] seatOccupied;

    // ���� �ɾ� �ִ� �¼� �ε��� (-1 : �ƹ� �¼��� �ɾ� ���� ����)
    private int currentSeatIndex = -1;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seatOccupied = new bool[seatPositions.Length];
    }

    /// <summary>
    /// �÷��̾ ������ ž�½�Ű�� �޼���
    /// </summary>
    public void BoardBus()
    {
        StartCoroutine(BoardRoutineCo());
    }

    /// <summary>
    /// �÷��̾ �������� ������Ű�� �޼���
    /// </summary>
    
    public void ExitBus()
    {
        StartCoroutine(ExitRoutineCo());
    }

    private IEnumerator BoardRoutineCo()
    {
        Debug.Log("ž�� ��ƾ ����");

        yield return FadeUIController.Instance.FadeOut();

        if (player == null)
        {
            Debug.LogError("player�� ã�� �� �����ϴ�.");
            yield break;
        }

        var controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        int seatIndex = FindAvailableSeatIndex();
        if (seatIndex == -1)
        {
            Debug.LogWarning("ž�� ������ �¼��� �����ϴ�.");
            yield return FadeUIController.Instance.FadeIn();
            yield break;
        }

        Transform seat = seatPositions[seatIndex];
        player.transform.position = seat.position;
        player.transform.rotation = seat.rotation;
        Debug.Log($"Player �̵� �Ϸ�: {player.transform.position}");

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
            seatOccupied[currentSeatIndex] = false; // ���� �¼� ����
            currentSeatIndex = -1; // �¼� �ε��� �ʱ�ȭ
        }

        if (controller != null) controller.enabled = true;

        if (locomotionProvider != null)
        {
            locomotionProvider.gameObject.SetActive(true);
        }

        yield return FadeUIController.Instance.FadeIn();
    }

    /// <summary>
    /// ��� �ִ� �¼� �ε����� ��ȯ (������ -1 ��ȯ)
    /// </summary>
    private int FindAvailableSeatIndex()
    {
        for (int i = 0; i < seatOccupied.Length; i++)
        {
            if (!seatOccupied[i])
            {
                return i; // ��� �ִ� �¼� �ε��� ��ȯ
            }
        }
        return -1; // ��� �¼��� ���� ���� ���
    }
}
