using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Splines;

public class WalkingNPC : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float surpriseDuration = 1.5f;
    [SerializeField] private float detectionRadious = 1f;
    [SerializeField] private float detectionOffsetX = 1f;
    [SerializeField] private float detectionOffsetY = 1f;
    [SerializeField] LayerMask obstacleLayer;

    private Animator animator;

    private float t = 0f;

    private bool isBlocked = false;
    private bool isSurprising = false;
    private bool isAlreadySurprised = false;

    void Start()
    {
        //if (!PhotonNetwork.IsMasterClient) return;

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
        //if (!PhotonNetwork.IsMasterClient) return;

        CheckObstacle();

        if (isBlocked)
        {
            Stop();
            StartCoroutine(SurpriseAndWait());
        }

        if(isBlocked || isSurprising)
        {
            return;
        }

        if (isAlreadySurprised)
        {
            isAlreadySurprised = false;
        }

        Move();
    }

    void Move()
    {
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
        Vector3 origin = transform.position + new Vector3(detectionOffsetX, detectionOffsetY, 0);
        float radius = detectionRadious;

        Collider[] hits = Physics.OverlapSphere(origin, radius, obstacleLayer);
        isBlocked = false; 
        
        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                isBlocked = true;
                break;
            }
        }
    }

    IEnumerator SurpriseAndWait()
    {
        if (isSurprising || isAlreadySurprised)
            yield break;

        isSurprising = true;
        isAlreadySurprised = true;
        animator.SetTrigger("Surprised");
        yield return new WaitForSeconds(surpriseDuration);

        while (isBlocked)
        {
            yield return null;
        }

        isSurprising = false;
    }

    void Stop()
    {
        animator.SetBool("isWalking", false);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + new Vector3(detectionOffsetX, detectionOffsetY, 0);
        float radius = detectionRadious;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, radius);
    }
}
