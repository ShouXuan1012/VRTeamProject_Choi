using UnityEngine;
using UnityEngine.UI;

public enum LoginState
{
    Loading,
    Error,
    Success
}
public enum SignUpState
{
    IdChecking,
    IdNotChecked,
    IdChecked,
    Loading,
    Error,
    Success
}
public class LoginUIController : MonoBehaviour
{
    [Header("Login 관련 UI")]
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject loginLoadingScreen;
    [SerializeField] private Text loginNoticeMessage;
    [SerializeField] private GameObject mainWindow;

    [Header("Sign Up 관련 UI")]
    [SerializeField] private GameObject signUpWindow;
    [SerializeField] private GameObject signUpScreen;
    [SerializeField] private GameObject signUpLoadingScreen;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button checkIdButton;
    [SerializeField] private Text signUpNoticeMessage;
    [SerializeField] private GameObject nicknameWindow;

    private void Start()
    {
        loginScreen.SetActive(true);
        loginLoadingScreen.SetActive(false);
        loginNoticeMessage.gameObject.SetActive(false);

        signUpScreen.SetActive(true);
        signUpLoadingScreen.SetActive(false);
        checkIdButton.interactable = true;
        signUpButton.interactable = false;
        signUpNoticeMessage.gameObject.SetActive(false);
    }
    public void SetUIByLoginState(LoginState state)
    {
        switch (state)
        {
            case LoginState.Loading:
                loginScreen.SetActive(false);
                loginLoadingScreen.SetActive(true);
                break;
            case LoginState.Error:
                loginScreen.SetActive(true);
                loginLoadingScreen.SetActive(false);
                break;
            case LoginState.Success:
                loginWindow.SetActive(false);
                mainWindow.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetUIBySignUpState(SignUpState state)
    {
        switch (state)
        {
            case SignUpState.IdChecking:
                checkIdButton.interactable = false;
                signUpButton.interactable = false;
                break;
            case SignUpState.IdNotChecked:
                checkIdButton.interactable = true;
                signUpButton.interactable = false;
                break;
            case SignUpState.IdChecked:
                checkIdButton.interactable = false;
                signUpButton.interactable = true;
                break;
            case SignUpState.Loading:
                signUpScreen.SetActive(false);
                signUpLoadingScreen.SetActive(true);
                break;
            case SignUpState.Error:
                signUpScreen.SetActive(true);
                signUpLoadingScreen.SetActive(false);
                break;
            case SignUpState.Success:
                signUpWindow.SetActive(false);
                nicknameWindow.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SetLoginNoticeMessage(string message, bool isError = true)
    {
        if (string.IsNullOrEmpty(message))
        {
            loginNoticeMessage.gameObject.SetActive(false);
            return;
        }

        loginNoticeMessage.gameObject.SetActive(true);
        loginNoticeMessage.color = isError ? Color.red : Color.green;
        loginNoticeMessage.text = message;
    }
    public void SetSignUpNoticeMessage(string message, bool isError = true)
    {
        if (string.IsNullOrEmpty(message))
        {
            signUpNoticeMessage.gameObject.SetActive(false);
            return;
        }

        signUpNoticeMessage.gameObject.SetActive(true);
        signUpNoticeMessage.color = isError ? Color.red : Color.green;
        signUpNoticeMessage.text = message;
    }
}
