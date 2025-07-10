using UnityEngine;
using Photon.Pun; // 멀티 붙일 때 다시 활성화

public class VRAvatarIKController : MonoBehaviourPun
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

    [Header("머리 타겟 (HMD)")]
    public Transform headTarget;

    [Header("머리 본 (아바타 머리 뼈)")]
    public Transform headBone;

    [SerializeField] private Transform chest;
    [SerializeField] private float maxHeadOffset = 0.3f;

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
        if (headTarget != null && headBone != null && chest != null)
        {
            Vector3 offset = headTarget.position - chest.position;

            if (offset.magnitude > maxHeadOffset)
                offset = offset.normalized * maxHeadOffset;

            headBone.position = chest.position + offset;
            headBone.rotation = headTarget.rotation;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
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
