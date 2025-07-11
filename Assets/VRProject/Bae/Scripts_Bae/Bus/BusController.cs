using System.Collections;
using UnityEngine;

/// <summary>
/// ���� ������ ��� ��ȸ �� ���� ���� ���� Ŭ����.
/// �̵��� ���°����� ���, UI �� ž�� ó���� �ٸ� Ŭ�������� ���.
/// </summary>
public class BusController : MonoBehaviour
{
    [Header("������ ��� ������� ����")]
    [SerializeField] private Transform[] busStops; // ���� ������ ��ġ �迭

    [Header("���� �ӵ� �� ������ ��� �ð�")]
    [SerializeField] private float busSpeed = 5f; // ���� �̵� �ӵ�
    [SerializeField] private float waitTime = 10f; // ������ ��� �ð�

    private int currentStopIndex = 0; // ���� ������ �ε���

    public bool IsWaitingAtStop { get; private set; } // ���� �����忡�� ��� ������ ����

    private void Start()
    {
        if (busStops.Length > 0)
        {
            transform.position = busStops[0].position; // �ʱ� ��ġ�� ù ��° ���������� ����
            StartCoroutine(BusRoutineCo());
        }
    }

    /// <summary>
    /// ������ �������� ������� ��ȸ�ϸ� ���� ��� ���¸� �����ϴ� �ڷ�ƾ.
    /// </summary>
    private IEnumerator BusRoutineCo()
    {
        while (true)
        {
            Transform targetStop = busStops[currentStopIndex];

            // ���� ���������� �̵�
            while (Vector3.Distance(transform.position, targetStop.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetStop.position, busSpeed * Time.deltaTime);
                yield return null; // ���� �����ӱ��� ���
            }

            // �����忡 ����
            transform.position = targetStop.position;
            IsWaitingAtStop = true; // ������ ��� ���·� ��ȯ

            yield return new WaitForSeconds(waitTime); // ��� �ð� ���� ���

            // ������ ��� ���� ����
            IsWaitingAtStop = false;
            currentStopIndex = (currentStopIndex + 1) % busStops.Length; // ���� ���������� �ε��� �̵�
        }
    }
}
