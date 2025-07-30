using UnityEngine;
using UnityEngine.UI;

public class LoginButtonHandler : MonoBehaviour
{
    [SerializeField] private LoginManager loginManager;

    [SerializeField] private Button loginButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button checkIdButton;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        signUpButton.onClick.AddListener(OnSignUpButtonClicked);
        checkIdButton.onClick.AddListener(OnCheckIdButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        loginManager.HandleLogin();
    }

    private void OnSignUpButtonClicked()
    {
        loginManager.HandleSignUp();
    }

    private void OnCheckIdButtonClicked()
    {
        loginManager.HandleCheckId();
    }
}
