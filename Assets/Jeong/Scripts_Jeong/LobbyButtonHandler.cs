using UnityEngine;

public class LobbyButtonHandler : MonoBehaviour
{
    [SerializeField] private LobbyManager lobbyManager;

    /// <summary>
    /// 입장 버튼 클릭 시 호출되는 메서드
    /// </summary>
    public void OnJoinRoomClicked()
    {
        lobbyManager.TryJoinRoom();
    }
}
