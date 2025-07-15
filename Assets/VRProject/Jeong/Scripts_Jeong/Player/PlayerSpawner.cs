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

            foreach (Transform newPart in newMesh)
            {
                Transform originalPart = meshContainer.Find(newPart.name);
                if (originalPart == null) continue;

                SkinnedMeshRenderer sourceRenderer = newPart.GetComponent<SkinnedMeshRenderer>();
                SkinnedMeshRenderer targetRenderer = originalPart.GetComponent<SkinnedMeshRenderer>();

                if (sourceRenderer != null && targetRenderer != null)
                {
                    Material[] sourceMaterials = sourceRenderer.sharedMaterials;
                    Material[] clonedMaterials = new Material[sourceMaterials.Length];

                    for (int i = 0; i < sourceMaterials.Length; i++)
                    {
                        clonedMaterials[i] = Instantiate(sourceMaterials[i]);
                    }

                    targetRenderer.sharedMesh = sourceRenderer.sharedMesh;
                    targetRenderer.materials = clonedMaterials;
                }
            }
        }
    }
}