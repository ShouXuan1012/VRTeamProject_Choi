using UnityEngine;

/// <summary>
/// 농구공 collider가 만약에 범위를 빠져나가면 다시 맵안으로 들어오게 설정
/// </summary>
public class OutOfBoundsTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;

        other.transform.position = respawnPoint.position;

        rb.useGravity = true;
    }
}
