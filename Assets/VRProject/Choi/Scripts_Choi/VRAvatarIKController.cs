using Photon.Pun;
using UnityEngine;

public class VRAvatarIKControllerPun2 : MonoBehaviourPun
{
    [Header("IK 타겟 (컨트롤러 위치)")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("내가 보는 손 모델 (FBX 따로 가져온 손)")]
    public Transform handModelLeft;
    public Transform handModelRight;

    [Header("아바타 전체")]
    public GameObject bodyMeshRoot;  // 전체 아바타(스킨 메시 포함)
    public GameObject handMeshRoot;  // 내가 보는 손 오브젝트 묶음

    [Header("왼손 오프셋")]
    public Vector3 leftHandRotationOffset;

    [Header("오른손 오프셋")]
    public Vector3 rightHandRotationOffset;


    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 멀티 체크 제거 → 항상 내 시점 기준
        if (photonView.IsMine)
        {
            if (bodyMeshRoot != null) bodyMeshRoot.SetActive(false);
            if (handMeshRoot != null) handMeshRoot.SetActive(true);
        }
        else
        {
            if (bodyMeshRoot != null) bodyMeshRoot.SetActive(true);
            if (handMeshRoot != null) handMeshRoot.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (!photonView.IsMine) return;

        if (handModelLeft != null && leftHandTarget != null)
        {
            handModelLeft.position = leftHandTarget.position;
            handModelLeft.rotation = leftHandTarget.rotation * Quaternion.Euler(leftHandRotationOffset);
        }

        if (handModelRight != null && rightHandTarget != null)
        {
            handModelRight.position = rightHandTarget.position;
            handModelRight.rotation = rightHandTarget.rotation * Quaternion.Euler(rightHandRotationOffset);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!photonView.IsMine || animator == null) return;
        if (animator == null) return;

        if (leftHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }

        if (rightHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
    }
}
