using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PhotonVRControllerGuard : MonoBehaviourPun
{
    public ActionBasedController leftController;
    public ActionBasedController rightController;

    void Start()
    {
        if (!photonView.IsMine)
        {
            // Disable input for remote players
            leftController.enableInputActions = false;
            rightController.enableInputActions = false;
        }

        if (!photonView.IsMine)
        {
            GetComponent<LocomotionSystem>().enabled = false;
            GetComponent<ContinuousMoveProviderBase>().enabled = false;
        }

        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }

    }
}
