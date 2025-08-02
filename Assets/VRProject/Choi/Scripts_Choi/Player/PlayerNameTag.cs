using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField] private Text nicknameText;
    [SerializeField] private Text titleText;

    private Camera mainCam;

    private void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.SetActive(false);
            return;
        }
        mainCam = Camera.main;

        
        nicknameText.text = photonView.Owner.NickName;
        titleText.text = photonView.Owner.CustomProperties.ContainsKey("titleName") ? 
            photonView.Owner.CustomProperties["titleName"].ToString() : "No Title";
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
