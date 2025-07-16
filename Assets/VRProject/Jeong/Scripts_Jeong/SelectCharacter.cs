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

        PlayerPrefs.SetString("SelectedCharacter", selectedName);
        PlayerPrefs.Save();
    }
}
