using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private CharacterPreviewManager characterPreviewManager;

    public void OnSelectButtonClicked()
    {
        string selectedName = characterPreviewManager.GetCurrentCharacterName();
        Debug.Log("Selected Character Index: " + selectedName);

        PlayerPrefs.SetString("SelectedCharacter", selectedName);
    }
}
