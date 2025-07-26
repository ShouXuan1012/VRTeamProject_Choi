using UnityEngine;
using UnityEngine.UI;

public class LoginUIController : MonoBehaviour
{
    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject loginLoadingScreen;
    [SerializeField] private GameObject loginErrorScreen;

    [SerializeField] private GameObject signUpScreen;
    [SerializeField] private GameObject signUpLoadingScreen;
    [SerializeField] private GameObject signUpErrorScreen;

    [SerializeField] private Button signUpButton;
    [SerializeField] private Button checkIdButton;
}
