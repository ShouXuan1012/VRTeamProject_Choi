using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private CharacterPreviewManager characterPreviewManager;

    [Header("캐릭터 선택 UI")]
    [SerializeField] private Button selectButton;
    [SerializeField] private Text characterNoticeMessage;

    [Header("전환할 창")]
    [SerializeField] private GameObject currentWindow;
    [SerializeField] private GameObject nextWindow;

    private void Start()
    {
        selectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    private async void OnSelectButtonClicked()
    {
        string newAvatar = characterPreviewManager.GetCurrentCharacterName();

        if (string.IsNullOrEmpty(newAvatar))
        {
            characterNoticeMessage.gameObject.SetActive(true);
            characterNoticeMessage.text = "캐릭터를 선택해주세요.";
            return;
        }

        bool isSaved = await CurrentUserManager.Instance.UpdateAvatar(newAvatar);
        if (isSaved)
        {
            CurrentUserManager.Instance.SetAvatar(newAvatar);

            currentWindow.SetActive(false);
            nextWindow.SetActive(true);
        }
        else
        {
            characterNoticeMessage.gameObject.SetActive(true);
            characterNoticeMessage.text = "캐릭터 저장에 실패했습니다.\n다시 시도해주세요.";
        }
    }
}
