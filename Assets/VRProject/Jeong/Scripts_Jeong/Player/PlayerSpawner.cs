using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

            Transform root = player.transform.Find("Character");
            Transform oldMesh = root.Find("Character/Mesh");
            if (oldMesh != null) Destroy(oldMesh.gameObject);

            string meshName = (string)PhotonNetwork.LocalPlayer.CustomProperties["SelectedCharacter"];
            GameObject meshPrefab = Resources.Load<GameObject>($"CharacterMesh/{meshName}");

            GameObject newMesh = Instantiate(meshPrefab, root);
            newMesh.name = "Mesh";
        }
    }
}