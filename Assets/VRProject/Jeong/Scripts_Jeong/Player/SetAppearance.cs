using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SetAppearance : MonoBehaviourPun
{
    void Start()
    {
        StartCoroutine(WaitAndApplyAppearance());
    }

    IEnumerator WaitAndApplyAppearance()
    {
        Photon.Realtime.Player owner = photonView.Owner;

        // 속성 반영될 때까지 대기
        while (!owner.CustomProperties.ContainsKey("SelectedCharacter"))
            yield return null;

        // 기존 캐릭터
        Transform root = transform.Find("Character");
        Transform meshContainer = root.Find("Mesh");

        // 선택한 캐릭터
        string selectedName = (string)owner.CustomProperties["SelectedCharacter"];
        GameObject meshSource = Resources.Load<GameObject>($"CharacterMesh/{selectedName}");
        Transform newMesh = meshSource.transform.Find("Mesh");

        // 각 파트의 속성 변경
        foreach (Transform newPart in newMesh)
        {
            Transform originalPart = meshContainer.Find(newPart.name);
            if (originalPart == null) continue;

            SkinnedMeshRenderer sourceRenderer = newPart.GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer targetRenderer = originalPart.GetComponent<SkinnedMeshRenderer>();

            if (sourceRenderer != null && targetRenderer != null)
            {
                // Mesh 변경
                targetRenderer.sharedMesh = sourceRenderer.sharedMesh;

                // Material 변경
                Material[] sourceMaterials = sourceRenderer.sharedMaterials;
                Material[] clonedMaterials = new Material[sourceMaterials.Length];
                for (int i = 0; i < sourceMaterials.Length; i++)
                {
                    clonedMaterials[i] = Instantiate(sourceMaterials[i]);
                }
                targetRenderer.materials = clonedMaterials;
            }
        }
    }
}
