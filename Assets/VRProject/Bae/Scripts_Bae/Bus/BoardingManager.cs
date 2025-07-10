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
    [SerializeField] private Transform seatPosition;

    [Header("���� ��ġ")]
    [SerializeField] private Transform exitPosition;

    [Header("�̵� ���� ��� ������Ʈ")]
    [SerializeField] private Behaviour locomotionProvider;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            Debug.LogError("player�� null�Դϴ�.");
            yield break;
        }

        var controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.transform.position = seatPosition.position;
        player.transform.rotation = seatPosition.rotation;
        Debug.Log($"XR Origin �̵� �Ϸ�: {player.transform.position}");

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
            locomotionProvider.enabled = true; // �̵� ���� ����
        }

        yield return FadeUIController.Instance.FadeIn();
    }

}
