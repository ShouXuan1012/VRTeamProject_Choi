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
                // 강제 이동을 위해 코루틴 시작
                StartCoroutine(MoveBall(rb));
            }
        }
    }
    
    private IEnumerator MoveBall(Rigidbody rb)
    {
        rb.velocity = Vector3.zero; // 튕기는 힘 제거
        rb.useGravity = false; // 중력 제거

        while (Vector3.Distance(rb.position, target.position) > 0.05f)
        {
            Vector3 dir = (target.position - rb.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
            yield return null;
        }

        rb.useGravity = true; // 다시 중력 활성화
    }
}
