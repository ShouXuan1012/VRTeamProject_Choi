using UnityEngine;

/*
리지드바디는 안쓰기
Root Motion 사용
정해진 경로로 이동
정해진 위치에 도달하면 잠깐 멈췄다가 방향 바꾸기
캐릭터 앞에 overlap 두고 그 안에 플레이어 있으면 정지
 */
public class NPC1 : MonoBehaviour
{
    [Header("Waypoint Settings")]
    [SerializeField] private Transform[] waypoints;
    private int currentIndex = 0;

    [Header("Movement Settings")]
    [SerializeField] private float waitTime = 2f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private float surpriseDuration = 1.5f;
    private bool isSurprised = false;
    private float surpriseTimer = 0f;


    [Header("Animation")]
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.LookAt(waypoints[currentIndex].position);
    }

    void Update()
    {
        // 놀람 상태 처리
        if (isSurprised)
        {
            surpriseTimer += Time.deltaTime;
            if (surpriseTimer >= surpriseDuration)
            {
                isSurprised = false;
                surpriseTimer = 0f;
            }
            return;
        }

        // 장애물 감지
        if (IsObstacleInFront())
        {
            animator.SetBool("isWalking", false);

            if (!isSurprised)
            {
                animator.SetTrigger("surprised");
                isSurprised = true;
            }
            return;
        }

        // 멈춤 처리
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentIndex = (currentIndex + 1) % waypoints.Length;
                transform.LookAt(waypoints[currentIndex].position);
            }
            else
            {
                animator.SetBool("isWalking", false);
                return;
            }
        }

        // 이동 처리 (Root Motion 기반)
        Vector3 targetPos = waypoints[currentIndex].position;
        Vector3 direction = targetPos - transform.position;
        direction.y = 0f;

        if (direction.magnitude < 0.2f)
        {
            isWaiting = true;
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }

    bool IsObstacleInFront()
    {
        Vector3 origin = transform.position + transform.forward * 1f;
        Collider[] hits = Physics.OverlapSphere(origin, detectionRadius, detectionLayers);
        return hits.Length > 0;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + transform.forward * 1f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, detectionRadius);
    }
}
