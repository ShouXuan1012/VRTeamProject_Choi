using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoLoader : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image profileImageUI;
    [SerializeField] private Text nicknameText;
    [SerializeField] private Text userIdText;
    [SerializeField] private Text joinDateText;

    private void OnEnable()
    {
        var user = CurrentUserManager.Instance.CurrentUserData;

        // 프로필 이미지 로드
        if (!string.IsNullOrEmpty(user.profileImage))
        {
            Sprite loadedSprite = Resources.Load<Sprite>($"CharacterImage/{user.profileImage}");
            if (loadedSprite != null)
                profileImageUI.sprite = loadedSprite;
            else
                Debug.LogWarning($"[PlayerInfoLoader] {user.profileImage} 이미지를 Resources에서 찾을 수 없습니다.");
        }

        // 텍스트 필드 채우기
        nicknameText.text = user.nickname;
        userIdText.text = user.userId;
        joinDateText.text = FormatJoinDate(user.signUpDate);
    }

    private string FormatJoinDate(Firebase.Firestore.Timestamp timestamp)
    {
        System.DateTime dateTime = timestamp.ToDateTime();
        return dateTime.ToString("yyyy.MM.dd") + " 가입";
    }
}