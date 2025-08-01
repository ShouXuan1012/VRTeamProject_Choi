using UnityEngine;

public class NetTrigger : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager; // 점수 관리 스크립트 참조
    [SerializeField] private Transform ballRespawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            int point = PlayerZoneTracker.IsInTwoPointZone ? 1 : 2;
            scoreManager.AddScore(point);

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
