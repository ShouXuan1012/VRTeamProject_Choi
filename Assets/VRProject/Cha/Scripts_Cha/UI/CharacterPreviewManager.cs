using UnityEngine;

public class CharacterPreviewManager : MonoBehaviour
{
    public Transform previewRoot;                // 캐릭터가 생성될 위치
    public GameObject[] characterPrefabs;        // 프리팹 배열
    private GameObject currentCharacter;
    private int currentIndex = 0;

    void Start()
    {
        ShowCharacter(0);
    }

    public void ShowNext()
    {
        currentIndex = (currentIndex + 1) % characterPrefabs.Length;
        ShowCharacter(currentIndex);
    }

    public void ShowPrevious()
    {
        currentIndex = (currentIndex - 1 + characterPrefabs.Length) % characterPrefabs.Length;
        ShowCharacter(currentIndex);
    }

    private void ShowCharacter(int index)
    {
        if (currentCharacter != null)
            Destroy(currentCharacter);

        currentCharacter = Instantiate(characterPrefabs[index], previewRoot.position, Quaternion.identity, previewRoot);
        currentCharacter.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}

