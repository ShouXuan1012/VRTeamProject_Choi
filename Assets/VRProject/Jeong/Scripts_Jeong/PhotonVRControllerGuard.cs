using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotonVRControllerGuard : MonoBehaviourPun
{
    [Header("내가 조작 가능한 오브젝트들")]
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;

    void Start()
    {
        if (!photonView.IsMine)
        {
            DisableOthers();
        }
    }

    void DisableOthers()
    {
        // 내 것만 카메라와 사운드 사용
        if (mainCamera.TryGetComponent(out Camera cam))
            cam.enabled = false;
        if (mainCamera.TryGetComponent(out AudioListener listener))
            listener.enabled = false;

        // 입력 막기
        DisableInput(leftHand);
        DisableInput(rightHand);
    }

    void DisableInput(GameObject hand)
    {
        if (hand.TryGetComponent(out ActionBasedController ctrl))
            ctrl.enabled = false;
        if (hand.TryGetComponent(out XRRayInteractor ray))
            ray.enabled = false;
        if (hand.TryGetComponent(out XRInteractorLineVisual visual))
            visual.enabled = false;
    }
}
