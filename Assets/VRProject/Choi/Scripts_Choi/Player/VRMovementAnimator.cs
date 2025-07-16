using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class VRMovementAnimator : MonoBehaviourPun
{
    [SerializeField] private ActionBasedContinuousMoveProvider moveProvider;
    [SerializeField] private float inputThreshold = 0.05f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!photonView.IsMine || moveProvider == null || animator == null) return;

        Vector2 moveInput = moveProvider.leftHandMoveAction.action.ReadValue<Vector2>();
        bool isMoving = moveInput.magnitude > inputThreshold;

        animator.SetBool("isMoving", isMoving); // 내 로컬 애니메이션 (동기화는 자동)
    }
}
