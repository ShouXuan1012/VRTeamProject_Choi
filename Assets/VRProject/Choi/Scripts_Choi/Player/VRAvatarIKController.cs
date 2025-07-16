using UnityEngine;
using Photon.Pun; // 멀티 붙일 때 다시 활성화

public class VRAvatarIKController : MonoBehaviourPun
{
    [Header("IK 타겟 (컨트롤러 위치)")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("내가 보는 손 모델 (FBX 따로 가져온 손)")]
    [SerializeField] private Transform handModelLeft;
    [SerializeField] private Transform handModelRight;

    [Header("아바타 전체")]
    [SerializeField] private GameObject bodyMeshRoot;  // 전체 아바타(스킨 메시 포함)
    [SerializeField] private GameObject handMeshRoot;  // 내가 보는 손 오브젝트 묶음

    [Header("왼손 오프셋")]
    [SerializeField] private Vector3 leftHandPositionOffset;
    [SerializeField] private Vector3 leftHandRotationOffset;

    [Header("오른손 오프셋")]
    [SerializeField] private Vector3 rightHandPositionOffset;
    [SerializeField] private Vector3 rightHandRotationOffset;

    [Header("머리 타겟 (HMD)")]
    [SerializeField] private Transform headTarget;

    [Header("머리 본 (아바타 머리 뼈)")]
    [SerializeField] private Transform headBone;
    [SerializeField] private Transform neck;
    [SerializeField] private float maxHeadOffset = 0.3f;

    [Header("손 본 (다른 사람에게 보이는 손)")]
    [SerializeField] private Transform leftHandBone;
    [SerializeField] private Transform rightHandBone;

    [Header("손 본 회전 오프셋만 적용")]
    [SerializeField] private Vector3 leftBoneRotationOffset;
    [SerializeField] private Vector3 rightBoneRotationOffset;


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
        float lerpSpeed = 15f;

        if (!photonView.IsMine)
        {
            // IK 타겟을 부드럽게 보간해서 움직이기 (다른 플레이어 기준)
            if (leftHandTarget != null)
            {
                leftHandTarget.position = Vector3.Lerp(leftHandTarget.position, leftHandTarget.position, Time.deltaTime * lerpSpeed); // 이건 의미 없음 (다시 설명 ↓)
                leftHandTarget.rotation = Quaternion.Slerp(leftHandTarget.rotation, leftHandTarget.rotation, Time.deltaTime * lerpSpeed);
            }

            if (rightHandTarget != null)
            {
                rightHandTarget.position = Vector3.Lerp(rightHandTarget.position, rightHandTarget.position, Time.deltaTime * lerpSpeed);
                rightHandTarget.rotation = Quaternion.Slerp(rightHandTarget.rotation, rightHandTarget.rotation, Time.deltaTime * lerpSpeed);
            }

            if (headTarget != null)
            {
                headTarget.position = Vector3.Lerp(headTarget.position, headTarget.position, Time.deltaTime * lerpSpeed);
                headTarget.rotation = Quaternion.Slerp(headTarget.rotation, headTarget.rotation, Time.deltaTime * lerpSpeed);
            }
        }

        // 손 본 회전
        if (leftHandBone != null && leftHandTarget != null)
        {
            leftHandBone.rotation = Quaternion.Slerp(leftHandBone.rotation, leftHandTarget.rotation * Quaternion.Euler(leftBoneRotationOffset), Time.deltaTime * lerpSpeed);
        }

        if (rightHandBone != null && rightHandTarget != null)
        {
            rightHandBone.rotation = Quaternion.Slerp(rightHandBone.rotation, rightHandTarget.rotation * Quaternion.Euler(rightBoneRotationOffset), Time.deltaTime * lerpSpeed);
        }

        // 손 모델
        if (handModelLeft != null && leftHandTarget != null)
        {
            Vector3 targetPos = leftHandTarget.position + leftHandTarget.TransformDirection(leftHandPositionOffset);
            Quaternion targetRot = leftHandTarget.rotation * Quaternion.Euler(leftHandRotationOffset);
            handModelLeft.position = Vector3.Lerp(handModelLeft.position, targetPos, Time.deltaTime * lerpSpeed);
            handModelLeft.rotation = Quaternion.Slerp(handModelLeft.rotation, targetRot, Time.deltaTime * lerpSpeed);
        }

        if (handModelRight != null && rightHandTarget != null)
        {
            Vector3 targetPos = rightHandTarget.position + rightHandTarget.TransformDirection(rightHandPositionOffset);
            Quaternion targetRot = rightHandTarget.rotation * Quaternion.Euler(rightHandRotationOffset);
            handModelRight.position = Vector3.Lerp(handModelRight.position, targetPos, Time.deltaTime * lerpSpeed);
            handModelRight.rotation = Quaternion.Slerp(handModelRight.rotation, targetRot, Time.deltaTime * lerpSpeed);
        }

        // 머리
        if (headTarget != null && neck != null && headBone != null)
        {
            Vector3 offset = headTarget.position - neck.position;

            if (offset.magnitude > maxHeadOffset)
                offset = offset.normalized * maxHeadOffset;

            headBone.position = neck.position + offset;

            float angle = Vector3.SignedAngle(neck.forward, headTarget.forward, Vector3.up);
            angle = Mathf.Clamp(angle, -90f, 90f);

            Quaternion limitedRotation = Quaternion.AngleAxis(angle, Vector3.up) * Quaternion.LookRotation(neck.forward);
            headBone.rotation = Quaternion.Slerp(headBone.rotation, limitedRotation, Time.deltaTime * lerpSpeed);
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
