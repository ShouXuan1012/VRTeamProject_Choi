using UnityEngine;

public class VRMovementAnimator : MonoBehaviour
{

    [Header("VR 움직임 체크용 Character Controller")]
    [SerializeField] private CharacterController characterController;

    [Header("속도 임계값 (정지 vs 이동 판단)")]
    [SerializeField] private float moveThreshold = 0.05f;

    private Animator avatarAnimator;
    private bool wasMoving = false;
    void Start()
    {
        avatarAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if (avatarAnimator == null || characterController == null) return;

        float speed = characterController.velocity.magnitude;
        bool isMoving = speed > moveThreshold;

        if (isMoving != wasMoving)
        {
            avatarAnimator.SetBool("isMoving", isMoving);
            wasMoving = isMoving;
        }
    }
}
