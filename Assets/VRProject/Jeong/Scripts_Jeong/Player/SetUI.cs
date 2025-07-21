using Photon.Pun;
using UnityEngine;

public class SetUI : MonoBehaviourPun
{
    [SerializeField] private GameObject uiRoot;

    void Awake()
    {
        if (photonView.IsMine)
        {
            if (uiRoot != null)
                uiRoot.SetActive(true);
        }
        else
        {
            if (uiRoot != null)
                uiRoot.SetActive(false);
        }
    }
}

