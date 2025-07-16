using UnityEngine;
using UnityEngine.UI;

public class NicknameDisplay : MonoBehaviour
{
    public InputField nicknameInputField;
    public Text nicknamePreviewText;

    void Start()
    {
        nicknameInputField.onValueChanged.AddListener(UpdateNicknamePreview);
    }

    void UpdateNicknamePreview(string newText)
    {
        nicknamePreviewText.text = newText;
    }
}