using UnityEngine;

public class LobbyButtonHandler : MonoBehaviour
{
    [SerializeField] private LobbyManager lobbyManager;

    /// <summary>
    /// ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    /// </summary>
    public void OnJoinRoomClicked()
    {
        lobbyManager.TryJoinRoom();
    }
}
