using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PhotonVRControllerGuard : MonoBehaviourPun
{
    [Header("���� ���� ������ ������Ʈ��")]
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private InputActionManager inputManager; // �߰�

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
        // ī�޶� & �����
        if (mainCamera.TryGetComponent(out Camera cam))
            cam.enabled = false;
        if (mainCamera.TryGetComponent(out AudioListener listener))
            listener.enabled = false;

        // �Է� ���� (��)
        DisableInput(leftHand);
        DisableInput(rightHand);

        // �� �Է¸� �������� InputActionManager ��Ȱ��ȭ
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
