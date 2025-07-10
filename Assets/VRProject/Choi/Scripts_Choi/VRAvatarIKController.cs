using UnityEngine;
using Photon.Pun; // ��Ƽ ���� �� �ٽ� Ȱ��ȭ

public class VRAvatarIKController : MonoBehaviourPun
{
    [Header("IK Ÿ�� (��Ʈ�ѷ� ��ġ)")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("���� ���� �� �� (FBX ���� ������ ��)")]
    public Transform handModelLeft;
    public Transform handModelRight;

    [Header("�ƹ�Ÿ ��ü")]
    public GameObject bodyMeshRoot;  // ��ü �ƹ�Ÿ(��Ų �޽� ����)
    public GameObject handMeshRoot;  // ���� ���� �� ������Ʈ ����

    [Header("�޼� ������")]
    public Vector3 leftHandRotationOffset;

    [Header("������ ������")]
    public Vector3 rightHandRotationOffset;

    [Header("�Ӹ� Ÿ�� (HMD)")]
    public Transform headTarget;

    [Header("�Ӹ� �� (�ƹ�Ÿ �Ӹ� ��)")]
    public Transform headBone;

    [SerializeField] private Transform chest;
    [SerializeField] private float maxHeadOffset = 0.3f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // ��Ƽ üũ ���� �� �׻� �� ���� ����
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
