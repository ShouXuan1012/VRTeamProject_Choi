using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System.Collections.Generic;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private LoginUIController uiController;

    [SerializeField] private InputField loginIdInputField;
    [SerializeField] private InputField loginPasswordInputField;

    [SerializeField] private InputField signUpIdInputField;
    [SerializeField] private InputField signUpPasswordInputField;

    private bool isIdExists;

    void Start()
    {
        signUpIdInputField.onValueChanged.AddListener(OnSignUpIdChanged);
    }

    public async void HandleLogin()
    {
        string loginId = loginIdInputField.text;
        string loginPassword = loginPasswordInputField.text;

        if (!IsValidId(loginId))
        {
            uiController.SetLoginNoticeMessage($"아이디가 유효하지 않습니다.");
            return;
        }
        if (!IsValidPassword(loginPassword))
        {
            uiController.SetLoginNoticeMessage("비밀번호가 유효하지 않습니다.");
            return;
        }

        uiController.SetUIByLoginState(LoginState.Loading);

        uiController.SetLoginNoticeMessage("아이디 검사 시작");
        isIdExists = await UserDataManager.Instance.CheckIdExists(loginId);
        if (!isIdExists)
        {
            uiController.SetLoginNoticeMessage("아이디가 존재하지 않습니다.");
            uiController.SetUIByLoginState(LoginState.Error);
            return;
        }

        // 아이디가 존재하는 경우 사용자 데이터를 가져와서 비밀번호 확인
        uiController.SetLoginNoticeMessage("데이터 가져오기 시작");
        UserData userData = await UserDataManager.Instance.GetUserData(loginId);
        if (userData == null)
        {
            uiController.SetLoginNoticeMessage("아이디가 존재하지 않습니다.");
            uiController.SetUIByLoginState(LoginState.Error);
            return;
        }
        if (userData.isOnline)
        {
            uiController.SetLoginNoticeMessage("이미 로그인된 상태입니다.\n다른 기기에서 로그아웃 후 다시 시도해주세요.");
            uiController.SetUIByLoginState(LoginState.Error);
            return;
        }
        if (userData.password != loginPassword)
        {
            uiController.SetLoginNoticeMessage("비밀번호가 일치하지 않습니다.");
            uiController.SetUIByLoginState(LoginState.Error);
            return;
        }

        uiController.SetLoginNoticeMessage("온라인 여부 업데이트 시작");
        bool isUpdated = await UserDataManager.Instance.UpdateIsOnline(loginId, true);
        if (!isUpdated)
        {
            uiController.SetLoginNoticeMessage("로그인 중 오류가 발생했습니다.\n다시 시도해주세요.");
            uiController.SetUIByLoginState(LoginState.Error);
            return;
        }

        CurrentUserManager.Instance.SetCurrentUserData(userData);
        CurrentUserManager.Instance.SetIsOnline(true);

        uiController.SetLoginNoticeMessage("최고점수 데이터 가져오기 시작");
        List<TopScoreData> topScoreList = await TopScoreDataManager.Instance.GetAllTopScoreData(loginId);
        if (topScoreList == null)
        {
            topScoreList = new List<TopScoreData>();
        }
        
        CurrentUserManager.Instance.SetTopScoreDict(topScoreList);

        uiController.SetUIByLoginState(LoginState.Success);
    }

    public async void HandleSignUp()
    {
        string signUpId = signUpIdInputField.text;
        string signUpPassword = signUpPasswordInputField.text;

        if (!IsValidId(signUpId))
        {
            uiController.SetSignUpNoticeMessage("아이디가 유효하지 않습니다.");
            return;
        }
        if (!IsValidPassword(signUpPassword))
        {
            uiController.SetSignUpNoticeMessage("비밀번호가 유효하지 않습니다.");
            return;
        }

        if (isIdExists)
        {
            uiController.SetSignUpNoticeMessage("아이디가 이미 존재합니다.\n다른 아이디를 사용해주세요.");
            return;
        }

        UserData userData = new UserData
        {
            userId = signUpId,
            password = signUpPassword,
            nickname = "Nickname",
            avatar = "Boy1_CharacterIcon",
            profileImage = "Boy1_ProfileImage",
            coin = 316000,
            isOnline = true,
            signUpDate = Timestamp.GetCurrentTimestamp()
        };

        // 로딩 표시 활성화
        uiController.SetUIBySignUpState(SignUpState.Loading);

        bool isSaved = await UserDataManager.Instance.SaveUserData(userData);
        if (!isSaved)
        {
            uiController.SetSignUpNoticeMessage("회원가입 중 오류가 발생했습니다.\n다시 시도해주세요.");
            uiController.SetUIBySignUpState(SignUpState.Error);
            return;
        }

        CurrentUserManager.Instance.SetCurrentUserData(userData);

        List<TopScoreData> topScoreList = await TopScoreDataManager.Instance.GetAllTopScoreData(signUpId);
        if (topScoreList == null)
        {
            topScoreList = new List<TopScoreData>();
        }

        CurrentUserManager.Instance.SetTopScoreDict(topScoreList);

        uiController.SetUIBySignUpState(SignUpState.Success);
    }

    public async void HandleCheckId()
    {
        string signUpId = signUpIdInputField.text;

        if (!IsValidId(signUpId))
        {
            uiController.SetSignUpNoticeMessage("아이디가 유효하지 않습니다.");
            return;
        }

        uiController.SetUIBySignUpState(SignUpState.IdChecking);

        isIdExists = await UserDataManager.Instance.CheckIdExists(signUpId);
        if (isIdExists)
        {
            uiController.SetSignUpNoticeMessage("이미 존재하는 아이디입니다.\n다른 아이디를 사용해주세요.");
            uiController.SetUIBySignUpState(SignUpState.IdNotChecked);
            return;
        }

        uiController.SetSignUpNoticeMessage("사용 가능한 아이디입니다.", false);
        uiController.SetUIBySignUpState(SignUpState.IdChecked);
    }

    private void OnSignUpIdChanged(string newId)
    {
        isIdExists = true; // 입력값이 변경되면 중복 여부 초기화
        uiController.SetUIBySignUpState(SignUpState.IdNotChecked);
    }

    private bool IsValidId(string id)
    {
        bool isValid = false;

        // 비어있으면 false
        if (string.IsNullOrEmpty(id))
        {
            isValid = false;
        }
        // 길이가 3자 이상 10자 이하가 아니면 false
        else if (id.Length < 3 || id.Length > 10)
        {
            isValid = false;
        }
        else
        {
            isValid = true;
        }

        return isValid;
    }
    private bool IsValidPassword(string password)
    {
        bool isValid = false;

        // 비어있으면 false
        if (string.IsNullOrEmpty(password))
        {
            isValid = false;
        }
        // 길이가 4자 이상 16자 이하가 아니면 false
        else if (password.Length < 4 || password.Length > 16)
        {
            isValid = false;
        }
        else
        {
            isValid = true;
        }

        return isValid;
    }
}
