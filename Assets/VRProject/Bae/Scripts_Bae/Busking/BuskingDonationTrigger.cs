using System.Collections;
using UnityEngine;
using Photon.Pun;

public class BuskingDonationTrigger : MonoBehaviourPun
{
    private GameObject donationCanvas;

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        PhotonView pv = player.GetComponent<PhotonView>();
        if (pv == null || !pv.IsMine) return;

        Transform found = player.transform.Find("UI/DonationCanvas");
        if (found != null)
        {
            donationCanvas = found.gameObject;
            donationCanvas.SetActive(false); // 시작 시 비활성화
            Debug.Log("[BuskingDonationTrigger] DonationCanvas 연결 성공");
        }
        else
        {
            Debug.LogWarning("[BuskingDonationTrigger] DonationCanvas를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsLocalPlayer(other)) return;

        donationCanvas?.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsLocalPlayer(other)) return;

        donationCanvas?.SetActive(false);
    }

    private bool IsLocalPlayer(Collider other)
    {
        PhotonView view = other.GetComponent<PhotonView>();
        return view != null && view.IsMine;
    }
}
