using UnityEngine;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] private ConnectionManager connectionManager;

    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        joinRoomButton.onClick.AddListener(OnJoinRoomClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnJoinRoomClicked()
    {
        connectionManager.TryJoinRoom();
    }
    private void OnQuitClicked()
    {
        Application.Quit();
    }
}
