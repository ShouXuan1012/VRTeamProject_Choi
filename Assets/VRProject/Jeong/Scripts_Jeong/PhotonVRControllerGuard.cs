using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PhotonVRControllerGuard : MonoBehaviourPun
{
    [Header("내가 조작 가능한 오브젝트들")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private InputActionManager inputManager; // 추가

    private void Awake()
    {
        inputManager = FindObjectOfType<InputActionManager>();
    }
    void Start()
    {
        Debug.Log($"[Guard] IsMine: {photonView.IsMine}");

        if (!photonView.IsMine)
        {
            DisableOthers();
        }
    }

    void DisableOthers()
    {
        // 카메라 & 오디오
        if (mainCamera.TryGetComponent(out Camera cam))
            cam.enabled = false;
        if (mainCamera.TryGetComponent(out AudioListener listener))
            listener.enabled = false;

        // 입력 막기 (손)
        DisableInput(leftHand);
        DisableInput(rightHand);

        // 내 입력만 켜지도록 InputActionManager 비활성화
        if (inputManager != null)
        {
            Debug.Log("[Guard] Disabling InputActionManager for non-owner.");
            inputManager.enabled = false;
        }
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
