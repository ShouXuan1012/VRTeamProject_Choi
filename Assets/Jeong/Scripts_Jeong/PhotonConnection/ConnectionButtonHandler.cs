using UnityEngine;

public class ConnectionButtonHandler : MonoBehaviour
{
    [SerializeField] private ConnectionManager lobbyManager;

    /// <summary>
    /// 입장 버튼 클릭 시 호출되는 메서드
    /// </summary>
    public void OnJoinRoomClicked()
    {
        lobbyManager.TryJoinRoom();
    }
}
