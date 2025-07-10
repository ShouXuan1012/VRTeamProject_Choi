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
            animator.SetBool("isMoving", isMoving); // �� �ִϸ��̼�
            photonView.RPC("SetMovingAnim", RpcTarget.Others, isMoving); // �ٸ� ������� ����ȭ
            wasMoving = isMoving;
        }
    }

    [PunRPC]
    public void SetMovingAnim(bool isMoving)
    {
        if (photonView.IsMine) return; // �� ĳ���ʹ� ���� ó�������Ƿ� ����
        if (animator == null) animator = GetComponent<Animator>();

        animator.SetBool("isMoving", isMoving);
    }
}
