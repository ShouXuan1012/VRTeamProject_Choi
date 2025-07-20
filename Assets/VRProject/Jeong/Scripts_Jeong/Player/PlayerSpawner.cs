using Photon.Pun;
using System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerSpawned;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.TagObject != null)
        {
            Debug.Log("[PlayerSpawner] 플레이어 이미 존재");
            return;
        }

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
        PhotonNetwork.LocalPlayer.TagObject = player;
        OnPlayerSpawned?.Invoke(player); // 여기서 신호 보내기
    }
}
