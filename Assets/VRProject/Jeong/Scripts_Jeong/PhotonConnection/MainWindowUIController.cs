using UnityEngine;
using UnityEngine.UI;

public enum LobbyState
{
    Default,
    Loading,
    Ready,
    Error
}
public class MainWindowUIController : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject errorScreen;

    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Text errorMessage;

    /// <summary>
    /// 로비 상태에 따라 UI 설정
    /// </summary>
    /// <param name="state"></param>
    public void SetUIByLobbyState(LobbyState state)
    {
        switch (state)
        {
            case LobbyState.Default:
                menuScreen.SetActive(true);
                loadingScreen.SetActive(false);
                errorScreen.SetActive(false);

                joinRoomButton.interactable = false;
                quitButton.interactable = true;
                break;

            case LobbyState.Loading:
                menuScreen.SetActive(false);
                loadingScreen.SetActive(true);
                errorScreen.SetActive(false);
                break;

            case LobbyState.Ready:
                menuScreen.SetActive(true);
                loadingScreen.SetActive(false);
                errorScreen.SetActive(false);

                joinRoomButton.interactable = true;
                quitButton.interactable = true;
                break;

            case LobbyState.Error:
                menuScreen.SetActive(false);
                loadingScreen.SetActive(false);
                errorScreen.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// 에러 메세지 업데이트
    /// </summary>
    /// <param name="message"></param>
    public void SetErrorMessage(string message)
    {
        if (errorMessage != null)
        {
            errorMessage.text = message;
        }
    }
}
