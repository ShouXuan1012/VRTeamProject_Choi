using Photon.Pun;

public class PlayerTagAssigner : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)
            PhotonNetwork.LocalPlayer.TagObject = gameObject;
    }
}
