using UnityEngine;
using UnityEngine.UI;

public enum LobbyUIState
{
    Initial,
    Loading,
    Ready,
    JoinedRoom
}
public class LobbyUIController : MonoBehaviour
{
    [SerializeField] private Text statusText;
    [SerializeField] private Image loadingImage;
    [SerializeField] private Button joinRoomButton;

    /// <summary>
    /// ���� �ؽ�Ʈ ������Ʈ
    /// </summary>
    /// <param name="message"></param>
    public void SetStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
    /// <summary>
    /// �κ� ���¿� ���� UI ���� ����
    /// </summary>
    /// <param name="state"></param>
    public void SetUIState(LobbyUIState state)
    {
        switch (state)
        {
            case LobbyUIState.Initial:
                statusText.gameObject.SetActive(true);
                loadingImage.gameObject.SetActive(false);
                joinRoomButton.interactable = false;
                break;
            case LobbyUIState.Loading:
                loadingImage.gameObject.SetActive(true);
                joinRoomButton.interactable = false;
                break;
            case LobbyUIState.Ready:
                loadingImage.gameObject.SetActive(false);
                joinRoomButton.interactable = true;
                break;
            case LobbyUIState.JoinedRoom:
                loadingImage.gameObject.SetActive(false);
                joinRoomButton.interactable = false;
                break;
        }
    }
}
