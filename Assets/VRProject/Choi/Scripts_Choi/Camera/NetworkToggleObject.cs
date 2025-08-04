using UnityEngine;
using Photon.Pun;

public class NetworkToggleObject : MonoBehaviourPun
{
    [SerializeField] private GameObject target;

    public void Toggle()
    {
        if (!photonView.IsMine) return; // 내 플레이어만 토글 가능

        bool newState = !target.activeSelf;
        photonView.RPC(nameof(RPC_ToggleObject), RpcTarget.AllBuffered, newState);
    }

    [PunRPC]
    void RPC_ToggleObject(bool state)
    {
        if (target != null)
            target.SetActive(state);
    }
}
