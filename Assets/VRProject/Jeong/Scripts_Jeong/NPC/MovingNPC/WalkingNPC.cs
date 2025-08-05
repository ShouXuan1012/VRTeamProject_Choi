using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Splines;

public class WalkingNPC : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float detectionDistance = 2f;
    [SerializeField] float detectionOffsetY = 1f;
    [SerializeField] LayerMask obstacleLayer;

    private Animator animator;

    private float t = 0f;
    private bool isStopped = false;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        animator = GetComponent<Animator>();
        if (spline == null)
        {
            Debug.LogError("SplineContainer is not assigned.");
            enabled = false;
            return;
        }
        t = 0f;
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        CheckObstacle();
        Move();
    }

    void Move()
    {
        if (isStopped) return;

        t += Time.deltaTime * moveSpeed / spline.CalculateLength();
        if (t > 1f) t -= 1f; // 루프 처리

        Vector3 position = spline.EvaluatePosition(t);
        Vector3 tangent = spline.EvaluateTangent(t);

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(tangent);

        animator.SetBool("isWalking", true);
    }

    void CheckObstacle()
    {
        Vector3 origin = transform.position + new Vector3(0, detectionOffsetY, 0) + transform.forward * detectionDistance * 0.5f;
        float radius = detectionDistance * 0.5f;

        Collider[] hits = Physics.OverlapSphere(origin, radius, obstacleLayer);
        if (hits.Length > 0)
        {
            StopAndReact();
        }
    }

    void StopAndReact()
    {
        isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Surprised");

        StartCoroutine(WaitUntilClear());
    }

    IEnumerator WaitUntilClear()
    {
        Vector3 origin = transform.position + new Vector3(0, detectionOffsetY, 0) + transform.forward * detectionDistance * 0.5f;
        float radius = detectionDistance * 0.5f;

        while (Physics.OverlapSphere(origin, radius, obstacleLayer).Length > 0)
        {
            yield return null;
        }

        isStopped = false;
        animator.SetBool("isWalking", true);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + new Vector3(0, detectionOffsetY, 0) + transform.forward * detectionDistance * 0.5f;
        float radius = detectionDistance * 0.5f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, radius);
    }
}
