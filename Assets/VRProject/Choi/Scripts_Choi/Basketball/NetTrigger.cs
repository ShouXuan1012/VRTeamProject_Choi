using UnityEngine;

public class NetTrigger : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager; // 점수 관리 스크립트 참조
    [SerializeField] private Transform ballRespawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            scoreManager.AddScore(1);

            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;

                other.transform.position = ballRespawnPoint.position;

                rb.useGravity = true;
            }
        }
    }
}
