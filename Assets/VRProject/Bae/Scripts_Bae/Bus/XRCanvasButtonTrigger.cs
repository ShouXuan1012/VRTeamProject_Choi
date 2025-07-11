using UnityEngine;

public class XRCanvasButtonTrigger : MonoBehaviour
{
    public enum ActionType { Board, Exit }

    [Header("���� ���� (ž�� / ����)")]
    [SerializeField] private ActionType actionType;

    [Header("World Space Canvas UI")]
    [SerializeField] private GameObject UICanvas;

    [Header("���� ��Ʈ�ѷ� (������ ��� ���� �Ǵܿ�)")]
    [SerializeField] private BusController busController;

    [Header("ž��/���� ��� ó�� Ŭ����")]
    [SerializeField] private BoardingManager boardingManager;

    private void Start()
    {
        if (UICanvas != null)
            UICanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // ���� ��ư�� ������ ��� ���� ���� Ȱ��ȭ
        if (actionType == ActionType.Exit)
        {
            if (busController != null && busController.IsWaitingAtStop)
            {
                UICanvas.SetActive(true);
            }
        }
        else
        {
            // ž���� ���� ���� Ȱ��ȭ (Ʈ���� �ȿ� �����⸸ �ϸ�)
            UICanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (UICanvas != null)
            UICanvas.SetActive(false);
    }

    public void OnButtonCliked()
    {
        if (actionType == ActionType.Board)
        {
            boardingManager.BoardBus();
        }
        else
        {
            boardingManager.ExitBus();
        }
    }
}