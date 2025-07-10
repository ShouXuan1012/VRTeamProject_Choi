using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class VRMovementAnimator : MonoBehaviourPun
{
    [SerializeField] private ActionBasedContinuousMoveProvider moveProvider;
    [SerializeField] private float inputThreshold = 0.05f;

    private Animator animator;
    private bool wasMoving = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!photonView.IsMine || moveProvider == null || animator == null) return;

        Vector2 moveInput = moveProvider.leftHandMoveAction.action.ReadValue<Vector2>();
        bool isMoving = moveInput.magnitude > inputThreshold;

        if (isMoving != wasMoving)
        {
            animator.SetBool("isMoving", isMoving); // 내 애니메이션
            photonView.RPC("SetMovingAnim", RpcTarget.Others, isMoving); // 다른 사람에게 동기화
            wasMoving = isMoving;
        }
    }

    [PunRPC]
    public void SetMovingAnim(bool isMoving)
    {
        if (photonView.IsMine) return; // 내 캐릭터는 직접 처리했으므로 제외
        if (animator == null) animator = GetComponent<Animator>();

        animator.SetBool("isMoving", isMoving);
    }
}
