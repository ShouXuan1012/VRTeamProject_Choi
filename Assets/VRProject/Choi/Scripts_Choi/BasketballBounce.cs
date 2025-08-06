using UnityEngine;

public class BasketballBounce : MonoBehaviour
{
    public float bounceForce = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Basketball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // 위 방향으로 힘을 가함
                Vector3 forceDirection = Vector3.down;
                ballRb.AddForce(forceDirection * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
