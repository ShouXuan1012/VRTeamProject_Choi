using UnityEngine;

public class CharacterPreviewManager : MonoBehaviour
{
    public Transform previewRoot;               // 캐릭터가 생성될 위치
    public GameObject[] characterPrefabs;        // 프리팹 배열
    private GameObject[] characterInstances;
    private int currentIndex = 0;

    void Start()
    {
        // 모든 캐릭터 미리 생성하고 비활성화
        characterInstances = new GameObject[characterPrefabs.Length];

        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            GameObject go = Instantiate(characterPrefabs[i], previewRoot);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(0, 180, 0);
            go.SetActive(false);
            characterInstances[i] = go;
        }

        // 첫 캐릭터만 활성화
        ShowCharacter(0);
    }

    public void ShowNext()
    {
        int nextIndex = (currentIndex + 1) % characterInstances.Length;
        ShowCharacter(nextIndex);
    }

    public void ShowPrevious()
    {
        int prevIndex = (currentIndex - 1 + characterInstances.Length) % characterInstances.Length;
        ShowCharacter(prevIndex);
    }

    private void ShowCharacter(int index)
    {
        if (characterInstances[currentIndex] != null)
            characterInstances[currentIndex].SetActive(false);

        characterInstances[index].SetActive(true);
        currentIndex = index;
    }
}