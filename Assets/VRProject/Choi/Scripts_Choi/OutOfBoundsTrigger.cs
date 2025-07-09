using UnityEngine;

/// <summary>
/// �󱸰� collider�� ���࿡ ������ ���������� �ٽ� �ʾ����� ������ ����
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
