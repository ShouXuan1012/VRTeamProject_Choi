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
            Transform meshContainer = root.Find("Mesh");

            string selectedName = (string)PhotonNetwork.LocalPlayer.CustomProperties["SelectedCharacter"];
            GameObject meshSource = Resources.Load<GameObject>($"CharacterMesh/{selectedName}");
            Transform newMesh = meshSource.transform.Find("Mesh");

            if (meshContainer != null && newMesh != null)
            {
                for (int i = meshContainer.childCount - 1; i >= 0; i--)
                {
                    Transform child = meshContainer.GetChild(i);
                    Destroy(child.gameObject);
                }
                foreach (Transform child in newMesh)
                {
                    GameObject newChild = Instantiate(child.gameObject, meshContainer);
                    newChild.name = child.name;
                }
            }
        }
    }
}