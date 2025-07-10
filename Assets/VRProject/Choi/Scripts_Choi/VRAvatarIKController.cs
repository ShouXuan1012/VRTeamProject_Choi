using UnityEngine;
using Photon.Pun; // ��Ƽ ���� �� �ٽ� Ȱ��ȭ

public class VRAvatarIKController : MonoBehaviourPun
{
    [Header("IK Ÿ�� (��Ʈ�ѷ� ��ġ)")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("���� ���� �� �� (FBX ���� ������ ��)")]
    [SerializeField] private Transform handModelLeft;
    [SerializeField] private Transform handModelRight;

    [Header("�ƹ�Ÿ ��ü")]
    [SerializeField] private GameObject bodyMeshRoot;  // ��ü �ƹ�Ÿ(��Ų �޽� ����)
    [SerializeField] private GameObject handMeshRoot;  // ���� ���� �� ������Ʈ ����

    [Header("�޼� ������")]
    [SerializeField] private Vector3 leftHandPositionOffset;
    [SerializeField] private Vector3 leftHandRotationOffset;

    [Header("������ ������")]
    [SerializeField] private Vector3 rightHandPositionOffset;
    [SerializeField] private Vector3 rightHandRotationOffset;

    [Header("�Ӹ� Ÿ�� (HMD)")]
    [SerializeField] private Transform headTarget;

    [Header("�Ӹ� �� (�ƹ�Ÿ �Ӹ� ��)")]
    [SerializeField] private Transform headBone;
    [SerializeField] private Transform neck;
    [SerializeField] private float maxHeadOffset = 0.3f;

    [Header("�� �� (�ٸ� ������� ���̴� ��)")]
    [SerializeField] private Transform leftHandBone;
    [SerializeField] private Transform rightHandBone;

    [Header("�� �� ȸ�� �����¸� ����")]
    [SerializeField] private Vector3 leftBoneRotationOffset;
    [SerializeField] private Vector3 rightBoneRotationOffset;


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

        // �Ķ��� �� (��): ��ġ�� �ǵ帮�� �ʰ� ȸ���� ���߱�
        if (leftHandBone != null && leftHandTarget != null)
        {
            leftHandBone.rotation = leftHandTarget.rotation * Quaternion.Euler(leftBoneRotationOffset);
        }

        if (rightHandBone != null && rightHandTarget != null)
        {
            rightHandBone.rotation = rightHandTarget.rotation * Quaternion.Euler(rightBoneRotationOffset);
        }

        // �Ͼ�� �� (�� �� ��)
        if (handModelLeft != null && leftHandTarget != null)
        {
            handModelLeft.position = leftHandTarget.position + leftHandTarget.TransformDirection(leftHandPositionOffset);
            handModelLeft.rotation = leftHandTarget.rotation * Quaternion.Euler(leftHandRotationOffset);
        }

        if (handModelRight != null && rightHandTarget != null)
        {
            handModelRight.position = rightHandTarget.position + rightHandTarget.TransformDirection(rightHandPositionOffset);
            handModelRight.rotation = rightHandTarget.rotation * Quaternion.Euler(rightHandRotationOffset);
        }

        // �Ӹ�
        if (headTarget != null && neck != null && headBone != null)
        {
            Vector3 offset = headTarget.position - neck.position;

            // �Ÿ� ���� (�̹� �ʰ� �ص� �� ����)
            if (offset.magnitude > maxHeadOffset)
                offset = offset.normalized * maxHeadOffset;

            headBone.position = neck.position + offset;

            // Y�� ȸ�� ���� (�� ���� -90~90��)
            float angle = Vector3.SignedAngle(neck.forward, headTarget.forward, Vector3.up);
            angle = Mathf.Clamp(angle, -90f, 90f);

            Quaternion limitedRotation = Quaternion.AngleAxis(angle, Vector3.up) * Quaternion.LookRotation(neck.forward);
            headBone.rotation = limitedRotation;
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
