using UnityEngine;

/// <summary>
/// ���� �չ� , �޹� ��ó Ʈ���� ������ ����, XR UI ��ư�� Ȱ��ȭ.
/// ������ ������ �����忡 ������ ��� ���� ����(��ư Ȱ��ȭ) ����.
/// </summary>
public class XRCanvasButtonTrigger : MonoBehaviour
{
    public enum ActionType
    {
        Board, // ž��
        Exit // ����
    }

    [Header("���� ���� (ž�� / ����)")]
    [SerializeField] private ActionType actionType;

    [Header("World Space Canvas UI")]
    [SerializeField] private GameObject UICanvas;

    [Header("���� ��Ʈ�ѷ� (������ ���� ���� Ȯ��)")]
    [SerializeField] private BusController busController;

    [Header("ž��/���� ��� ó�� Ŭ����")]
    [SerializeField] private BoardingManager boardingManager;

    private void Start()
    {
        if (UICanvas != null)
        {
            UICanvas.SetActive(false); // ���� �� UI ��ư ��Ȱ��ȭ
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ Ʈ���� ������ ������ UI ��ư Ȱ��ȭ
        if (!other.CompareTag("Player")) return;

        // ���� ��ư�� ������ ��� ���� ���� Ȱ��ȭ
        if (actionType == ActionType.Exit && !busController.IsWaitingAtStop)
        {
            UICanvas.SetActive(false);
            return;
        }

        UICanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ Ʈ���� ������ ����� UI ��ư ��Ȱ��ȭ
            UICanvas.SetActive(false);
        }
    }

    /// <summary>
    /// XR UI ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    /// </summary>
    public void OnButtonCliked()
    {
        if (actionType == ActionType.Board)
        {
            boardingManager.BoardBus(); // ž��
        }
        else
        {
            boardingManager.ExitBus(); // ����
        }
    }
}
