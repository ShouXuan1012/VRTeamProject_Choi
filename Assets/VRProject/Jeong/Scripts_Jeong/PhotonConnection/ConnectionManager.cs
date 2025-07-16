using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private MainWindowUIController uiController;
    [SerializeField] private string sceneName = "MainGameScene";

    // 포톤 서버 설정
    private string gameVersion = "1";
    private string region = "kr";

    private void Start()
    {
        uiController.SetUIByLobbyState(LobbyState.Default);

        EnsureConnected();
    }

    /// <summary>
    /// 서버에 연결되어 있는지 확인 및 연결되지 않은 경우 연결 시도
    /// </summary>
    private void EnsureConnected()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            uiController.SetUIByLobbyState(LobbyState.Loading);

            ConnectToMasterServer();
        }
        else
        {
            uiController.SetUIByLobbyState(LobbyState.Ready);
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
        uiController.SetUIByLobbyState(LobbyState.Loading);

        ConnectToMasterServer();
    }
    public override void OnConnectedToMaster()
    {
        uiController.SetUIByLobbyState(LobbyState.Ready);
    }

    /// <summary>
    /// 룸 입장 시도
    /// </summary>
    public void TryJoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            uiController.SetUIByLobbyState(LobbyState.Loading);

            JoinOrCreateRoom();
        }
        else
        {
            uiController.SetUIByLobbyState(LobbyState.Loading);

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
        uiController.SetErrorMessage("입장에 실패했습니다.");
        uiController.SetUIByLobbyState(LobbyState.Error);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        uiController.SetErrorMessage("입장에 실패했습니다.");
        uiController.SetUIByLobbyState(LobbyState.Error);
    }
    public override void OnJoinedRoom()
    {
        uiController.SetUIByLobbyState(LobbyState.Loading);


        string selectedName = PlayerPrefs.GetString("SelectedCharacter");
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "SelectedCharacter", selectedName } });

        PhotonNetwork.LoadLevel(sceneName);
    }
}