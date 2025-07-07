using System.Collections;
using UnityEngine;

public class BackboardMagnet : MonoBehaviour
{
    public Transform target; // HoopCenter
    public float moveSpeed = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                // ���� �̵��� ���� �ڷ�ƾ ����
                StartCoroutine(MoveBall(rb));
            }
        }
    }
    
    private IEnumerator MoveBall(Rigidbody rb)
    {
        rb.velocity = Vector3.zero; // ƨ��� �� ����
        rb.useGravity = false; // �߷� ����

        while (Vector3.Distance(rb.position, target.position) > 0.05f)
        {
            Vector3 dir = (target.position - rb.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
            yield return null;
        }

        rb.useGravity = true; // �ٽ� �߷� Ȱ��ȭ
    }
}
