using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����忡 �÷��̾ �������� �� ���� ��ư Ȱ��ȭ
/// ���� ��Ʈ�ѷ��� ��� ���� ���� Ȱ��ȭ.
/// </summary>
public class BusStopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject exitButtonCanvas; // ���� ��ư UI
    [SerializeField] private BusController busController; // ���� ��Ʈ�ѷ� (������ ��� ���� �Ǵܿ�)

    void Start()
    {
        if (exitButtonCanvas != null)
            exitButtonCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // ������ ������ ��� ���� ���� ���� ��ư Ȱ��ȭ
        if (busController != null && busController.IsWaitingAtStop)
        {
            exitButtonCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // �÷��̾ ������ Ʈ���Ÿ� ����� ���� ��ư ��Ȱ��ȭ
        if (exitButtonCanvas != null)
            exitButtonCanvas.SetActive(false);
    }
}
