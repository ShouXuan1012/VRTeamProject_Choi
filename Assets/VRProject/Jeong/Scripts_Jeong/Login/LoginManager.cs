using UnityEngine;
using UnityEngine.UI;

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

        isIdExists = await UserDataManager.Instance.CheckIdExists(loginId);
        if (!isIdExists)
        {
            uiController.SetLoginNoticeMessage("아이디가 존재하지 않습니다.");
            uiController.SetUIByLoginState(LoginState.Error);
            return;
        }
        else
        {
            // 아이디가 존재하는 경우 사용자 데이터를 가져와서 비밀번호 확인
            UserData userData = await UserDataManager.Instance.GetUserData(loginId);
            if (userData != null && userData.password == loginPassword)
            {
                uiController.SetUIByLoginState(LoginState.Success);
                CurrentUserManager.Instance.SetCurrentUserData(userData);
            }
            else
            {
                uiController.SetLoginNoticeMessage("비밀번호가 일치하지 않습니다.");
                uiController.SetUIByLoginState(LoginState.Error);
            }
        }
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

        UserData newUser = new UserData
        {
            userId = signUpId,
            password = signUpPassword,
            nickname = "Nickname",
            avatar = "Boy1_CharacterIcon",
            coin = 316000
        };

        TopScoreData defaultTopScore1 = new TopScoreData
        {
            gameId = "basketball",
            score = 0
        };
        TopScoreData defaultTopScore2 = new TopScoreData
        {
            gameId = "bowling",
            score = 0
        };

        // 로딩 표시 활성화
        uiController.SetUIBySignUpState(SignUpState.Loading);

        bool isSaved = await UserDataManager.Instance.SaveUserData(newUser);
        if (!isSaved)
        {
            uiController.SetSignUpNoticeMessage("회원가입 중 오류가 발생했습니다.\n다시 시도해주세요.");
            uiController.SetUIBySignUpState(SignUpState.Error);
            return;
        }
        else
        {
            // TopScoreDataManager를 통해 기본 점수 데이터 저장
            bool isTopScoreSaved1 = await TopScoreDataManager.Instance.SaveTopScoreData(newUser.userId, defaultTopScore1.gameId, defaultTopScore1);
            bool isTopScoreSaved2 = await TopScoreDataManager.Instance.SaveTopScoreData(newUser.userId, defaultTopScore2.gameId, defaultTopScore2);
            // TopScore 저장은 성공 여부 체크 안함

            uiController.SetUIBySignUpState(SignUpState.Success);
            CurrentUserManager.Instance.SetCurrentUserData(newUser);
        }
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
        }
        else
        {
            uiController.SetSignUpNoticeMessage("사용 가능한 아이디입니다.", false);
            uiController.SetUIBySignUpState(SignUpState.IdChecked);
        }
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
