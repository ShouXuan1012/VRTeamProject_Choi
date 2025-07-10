using UnityEngine;

public class VRMovementAnimator : MonoBehaviour
{

    [Header("VR ������ üũ�� Character Controller")]
    [SerializeField] private CharacterController characterController;

    [Header("�ӵ� �Ӱ谪 (���� vs �̵� �Ǵ�)")]
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
