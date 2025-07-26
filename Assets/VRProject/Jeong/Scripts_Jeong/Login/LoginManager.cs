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
        // 예: 사용자 인증, 데이터베이스 조회 등
        string loginId = loginIdInputField.text;
        string loginPassword = loginPasswordInputField.text;

        if (!IsValidId(loginId))
        {
            Debug.LogError($"아이디가 유효하지 않습니다.: {loginId}");
            return;
        }
        if (!IsValidPassword(loginPassword))
        {
            Debug.LogError("비밀번호가 유효하지 않습니다.");
            return;
        }

        // 로딩 표시 활성화

        isIdExists = await UserDataManager.Instance.CheckIdExists(loginId);
        if (!isIdExists)
        {
            Debug.LogError($"로그인 실패: 아이디가 존재하지 않습니다.: {loginId}");
            // 에러 메시지 표시 후 로그인 창으로 돌아가기
            return;
        }
        else
        {
            // 아이디가 존재하는 경우, 사용자 데이터를 가져와서 비밀번호 확인
            UserData userData = await UserDataManager.Instance.GetUserData(loginId);
            if (userData != null && userData.password == loginPassword)
            {
                Debug.Log("로그인 성공");
                // 메인 메뉴 창 활성화
            }
            else
            {
                Debug.LogError("로그인 실패: 비밀번호가 잘못되었습니다.");
                // 에러 메시지 표시 후 로그인 창으로 돌아가기
            }
        }
    }

    public async void HandleSignUp()
    {
        // 예: 사용자 정보 입력, 데이터베이스 저장 등
        string signUpId = signUpIdInputField.text;
        string signUpPassword = signUpPasswordInputField.text;

        if (!IsValidId(signUpId))
        {
            Debug.LogError($"아이디가 유효하지 않습니다.: {signUpId}");
            return;
        }
        if (!IsValidPassword(signUpPassword))
        {
            Debug.LogError("비밀번호가 유효하지 않습니다.");
            return;
        }

        if (isIdExists)
        {
            Debug.LogError("아이디가 이미 존재합니다. 다른 아이디를 사용해주세요.");
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

        // 로딩 표시 활성화

        bool isSaved = await UserDataManager.Instance.SaveUserData(newUser);
        if (!isSaved)
        {
            Debug.LogError("회원가입 실패: 사용자 데이터를 저장하는 중 오류 발생");
            // 에러 메시지 표시 후 회원가입 창으로 돌아가기
            return;
        }
        else
        {
            Debug.Log("회원가입 성공");
            // 회원가입 성공 메시지 표시 후 닉네임 창 활성화
        }
    }

    public async void HandleCheckId()
    {
        string signUpId = signUpIdInputField.text;

        if (!IsValidId(signUpId))
        {
            Debug.LogError($"아이디가 유효하지 않습니다.: {signUpId}");
            return;
        }

        // 로딩 표시 활성화

        isIdExists = await UserDataManager.Instance.CheckIdExists(signUpId);
        if (isIdExists)
        {
            Debug.LogError("아이디가 이미 존재합니다. 다른 아이디를 사용해주세요.");
            // 아이디 중복 메시지 표시
        }
        else
        {
            Debug.Log("아이디 사용 가능");
            // 회원가입 버튼 활성화
        }
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

    private void OnSignUpIdChanged(string newId)
    {
        isIdExists = true; // 아이디가 변경되면 중복 여부 초기화
        // 회원가입 버튼 활성화
    }
}
