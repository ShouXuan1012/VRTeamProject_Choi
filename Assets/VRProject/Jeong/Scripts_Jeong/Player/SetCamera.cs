using Photon.Pun;
using UnityEngine;

public class SetCamera : MonoBehaviourPun
{
    public Camera playerCamera;

    void Awake()
    {
        if (photonView.IsMine)
        {
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
        }
    }
}
