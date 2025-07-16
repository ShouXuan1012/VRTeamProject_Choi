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

    public void OnJoinRoomClicked()
    {
        connectionManager.TryJoinRoom();
    }
    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
