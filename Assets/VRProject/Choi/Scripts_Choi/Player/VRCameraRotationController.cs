using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class VRCameraRotationController : MonoBehaviourPun
{
    [SerializeField] private Transform characterBody;    // 회전시킬 몸 오브젝트 (예: Character)
    [SerializeField] private Transform headTransform;    // HMD, Main Camera
    [SerializeField] private ActionBasedContinuousMoveProvider moveProvider; // 이동 시스템
    [SerializeField] private float rotationSpeed = 5f;   // 부드러운 회전 속도

    void LateUpdate()
    {
        if (!photonView.IsMine) return;

        // 조이스틱 입력값
        Vector2 moveInput = moveProvider.leftHandMoveAction.action.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 lookForward = headTransform.forward;
            lookForward.y = 0;
            lookForward.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(lookForward);
            characterBody.rotation = Quaternion.Slerp(characterBody.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
