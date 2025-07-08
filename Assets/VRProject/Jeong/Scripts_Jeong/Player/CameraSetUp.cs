using Photon.Pun;
using UnityEngine;

public class CameraSetUp : MonoBehaviourPun
{
    public Camera playerCamera;

    void Start()
    {
        if (photonView.IsMine)
            playerCamera.gameObject.SetActive(true);
    }
}
