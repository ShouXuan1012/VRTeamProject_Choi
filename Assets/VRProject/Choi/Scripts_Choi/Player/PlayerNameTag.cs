using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameTag : MonoBehaviourPunCallbacks
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
        UpdateTitleText(photonView.Owner);
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // 해당 플레이어의 titleName이 변경됐을 때만 반영
        if (targetPlayer == photonView.Owner && changedProps.ContainsKey("titleName"))
        {
            UpdateTitleText(targetPlayer);
        }
    }
    private void UpdateTitleText(Player player)
    {
        if (player.CustomProperties.ContainsKey("titleName"))
        {
            titleText.text = player.CustomProperties["titleName"].ToString();
        }
        else
        {
            titleText.text = "No Title";
        }
    }
}