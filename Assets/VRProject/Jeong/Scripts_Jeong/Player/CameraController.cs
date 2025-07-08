using Photon.Pun;
using UnityEngine;

public class CameraController : MonoBehaviourPun
{
    public Camera playerCamera;

    void Start()
    {
        if (photonView.IsMine)
            playerCamera.gameObject.SetActive(true);
    }
}
