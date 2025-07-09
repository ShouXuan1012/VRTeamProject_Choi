using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private ConnectionUIController ui;

    // 포톤 서버 설정
    private string gameVersion = "1";
    private string region = "kr";

    private void Start()
    {
        ui.SetUIState(LobbyUIState.Default);

        EnsureConnected();
    }

    /// <summary>
    /// 서버에 연결되어 있는지 확인 및 연결되지 않은 경우 연결 시도
    /// </summary>
    private void EnsureConnected()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            ui.SetStatusText("서버 연결 중...");
            ui.SetUIState(LobbyUIState.Loading);

            ConnectToMasterServer();
        }
        else
        {
            ui.SetStatusText("서버에 연결되어 있습니다.");
            ui.SetUIState(LobbyUIState.Ready);
        }
    }
    /// <summary>
    /// 포톤 서버에 연결
    /// </summary>
    private void ConnectToMasterServer()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            return;

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
        PhotonNetwork.ConnectUsingSettings();
    }

    // 서버 연결 관련 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        ui.SetStatusText("서버 연결이 끊어졌습니다. 재연결 중...");
        ui.SetUIState(LobbyUIState.Loading);

        ConnectToMasterServer();
    }
    public override void OnConnectedToMaster()
    {
        ui.SetStatusText("서버에 연결되었습니다.");
        ui.SetUIState(LobbyUIState.Ready);
    }

    /// <summary>
    /// 룸 입장 시도
    /// </summary>
    public void TryJoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            ui.SetStatusText("룸 입장 중...");
            ui.SetUIState(LobbyUIState.Loading);

            JoinOrCreateRoom();
        }
        else
        {
            ui.SetStatusText("서버에 연결되어 있지 않습니다. 재연결 중... 잠시 후 다시 시도해주세요.");
            ui.SetUIState(LobbyUIState.Loading);

            ConnectToMasterServer();
        }
    }
    /// <summary>
    /// 기본 룸으로 입장 시도 및 없을 경우 룸 생성
    /// </summary>
    private void JoinOrCreateRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        PhotonNetwork.JoinOrCreateRoom(
            "DefaultRoom",
            new RoomOptions { MaxPlayers = 20 },
            TypedLobby.Default
        );
    }

    // 룸 관련 콜백
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ui.SetStatusText("룸 입장에 실패했습니다. 다시 시도해주세요.");
        ui.SetUIState(LobbyUIState.Ready);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ui.SetStatusText("룸 생성에 실패했습니다. 다시 시도해주세요.");
        ui.SetUIState(LobbyUIState.Ready);
    }
    public override void OnJoinedRoom()
    {
        ui.SetStatusText("룸에 성공적으로 입장했습니다.");
        ui.SetUIState(LobbyUIState.Default);

        PhotonNetwork.LoadLevel("MainGameScene");
    }
}