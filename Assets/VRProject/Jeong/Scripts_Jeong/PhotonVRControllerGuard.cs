using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;


public class PhotonVRControllerGuard : MonoBehaviourPun
{
    [Header("XR Controllers")]
    public ActionBasedController leftController;
    public ActionBasedController rightController;

    [Header("XR Components")]
    public LocomotionSystem locomotionSystem;
    public ContinuousMoveProviderBase moveProvider;
    public Camera playerCamera;
    public AudioListener audioListener;
    private void Start()
    {
        if (!photonView.IsMine)
        {
            // Disable input on other clients
            if (leftController != null) leftController.enableInputActions = false;
            if (rightController != null) rightController.enableInputActions = false;

            // Disable movement systems
            if (locomotionSystem != null) locomotionSystem.enabled = false;
            if (moveProvider != null) moveProvider.enabled = false;

            // Disable camera and audio for remote players
            if (playerCamera != null) playerCamera.enabled = false;
            if (audioListener != null) audioListener.enabled = false;
        }
    }
}
