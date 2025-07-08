using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotonVRControllerGuard : MonoBehaviourPun
{
    [Header("���� ���� ������ ������Ʈ��")]
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
        // �� �͸� ī�޶�� ���� ���
        if (mainCamera.TryGetComponent(out Camera cam))
            cam.enabled = false;
        if (mainCamera.TryGetComponent(out AudioListener listener))
            listener.enabled = false;

        // �Է� ����
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
