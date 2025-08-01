using UnityEngine;
using UnityEngine.UI;

public class ChangeNickname : MonoBehaviour
{
    [Header("닉네임 변경 UI")]
    [SerializeField] private Button changeButton;
    [SerializeField] private InputField nicknameInputField;
    [SerializeField] private Text nicknameNoticeMessage;

    [Header("전환할 창")]
    [SerializeField] private GameObject currentWindow;
    [SerializeField] private GameObject nextWindow;

    void Start()
    {
        changeButton.onClick.AddListener(OnChangeButtonClicked);
    }

    private async void OnChangeButtonClicked()
    {
        string newNickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(newNickname))
        {
            nicknameNoticeMessage.gameObject.SetActive(true);
            nicknameNoticeMessage.text = "닉네임은 비워둘 수 없습니다.";
            return;
        }

        bool isSaved = await CurrentUserManager.Instance.UpdateNickname(newNickname);
        if (!isSaved)
        {
            nicknameNoticeMessage.gameObject.SetActive(true);
            nicknameNoticeMessage.text = "닉네임 저장에 실패했습니다.\n다시 시도해주세요.";
            return;
        }

        CurrentUserManager.Instance.SetNickname(newNickname);

        currentWindow.SetActive(false);
        nextWindow.SetActive(true);
    }
}
