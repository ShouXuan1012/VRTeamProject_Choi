using Photon.Pun;
using UnityEngine;

public class SetCamera : MonoBehaviourPun
{
    public Camera playerCamera;

    void Awake()
    {
        if (photonView.IsMine)
        {
            playerCamera.enabled = true;
            var listener = playerCamera.GetComponent<AudioListener>();
            if (listener != null) listener.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
            var listener = playerCamera.GetComponent<AudioListener>();
            if (listener != null) listener.enabled = false;
        }
    }
}
