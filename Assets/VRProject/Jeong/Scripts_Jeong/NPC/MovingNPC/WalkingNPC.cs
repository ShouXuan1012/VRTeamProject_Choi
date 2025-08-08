using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Splines;

public class WalkingNPC : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] private float surpriseDuration = 1.5f;
    [SerializeField] private float detectionRadious = 1f;
    [SerializeField] private float detectionOffsetForward = 1f;
    [SerializeField] private float detectionOffsetUp = 1f;
    [SerializeField] LayerMask obstacleLayer;

    private Animator animator;

    private float moveSpeed = 2f;
    private float t = 0f;

    private bool isBlocked = false;
    private bool isSurprising = false;
    private bool isAlreadySurprised = false;

    void Start()
    {
        // 포톤 뷰 있을 경우에만 마스터 클라이언트에서 처리
        if (GetComponent<PhotonView>() != null)
        {
            if (!PhotonNetwork.IsMasterClient) return;
        }

        animator = GetComponent<Animator>();
        if (spline == null)
        {
            Debug.LogError("SplineContainer is not assigned.");
            enabled = false;
            return;
        }
        moveSpeed = Random.Range(1.5f, 2.5f);
        t = 0f;
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (GetComponent<PhotonView>() != null)
        {
            if (!PhotonNetwork.IsMasterClient) return;
        }

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

        // y축 회전만 반영하도록 tangent의 y값 제거
        Vector3 flatTangent = new Vector3(tangent.x, 0f, tangent.z);
        if (flatTangent == Vector3.zero) flatTangent = transform.forward; // 안전 처리

        transform.position = position;
        transform.rotation = Quaternion.LookRotation(flatTangent);

        animator.SetBool("isWalking", true);
        animator.speed = moveSpeed / 2f;
    }

    void CheckObstacle()
    {
        Vector3 origin = transform.position + transform.forward * detectionOffsetForward + transform.up * detectionOffsetUp;
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
        Vector3 origin = transform.position + transform.forward * detectionOffsetForward + transform.up * detectionOffsetUp;
        float radius = detectionRadious;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, radius);
    }
}
