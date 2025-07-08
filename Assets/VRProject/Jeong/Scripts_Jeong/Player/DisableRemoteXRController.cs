using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableRemoteXRController : MonoBehaviourPun
{
    public XRBaseController gazeInteractor;
    public XRBaseController leftController;
    public XRBaseController rightController;

    void Awake()
    {
        if (!photonView.IsMine)
        {
            gazeInteractor.enabled = false;
            leftController.enabled = false;
            rightController.enabled = false;
        }
    }
}
