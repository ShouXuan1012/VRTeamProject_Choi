using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private LobbyUIController ui;

    // ���� ���� ����
    private string gameVersion = "1";
    private string region = "kr";

    private void Start()
    {
        ui.SetUIState(LobbyUIState.Initial);

        EnsureConnected();
    }

    /// <summary>
    /// ������ ����Ǿ� �ִ��� Ȯ�� �� ������� ���� ��� ���� �õ�
    /// </summary>
    private void EnsureConnected()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            ui.SetStatusText("���� ���� ��...");
            ui.SetUIState(LobbyUIState.Loading);

            ConnectToMasterServer();
        }
        else
        {
            ui.SetStatusText("������ ����Ǿ� �ֽ��ϴ�.");
            ui.SetUIState(LobbyUIState.Ready);
        }
    }
    /// <summary>
    /// ���� ������ ����
    /// </summary>
    private void ConnectToMasterServer()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            return;

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ���� ���� �ݹ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        ui.SetStatusText("���� ������ ���������ϴ�. �翬�� ��...");
        ui.SetUIState(LobbyUIState.Loading);

        ConnectToMasterServer();
    }
    public override void OnConnectedToMaster()
    {
        ui.SetStatusText("������ ����Ǿ����ϴ�.");
        ui.SetUIState(LobbyUIState.Ready);
    }

    /// <summary>
    /// �� ���� �õ�
    /// </summary>
    public void TryJoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            ui.SetStatusText("�� ���� ��...");
            ui.SetUIState(LobbyUIState.Loading);

            JoinOrCreateRoom();
        }
        else
        {
            ui.SetStatusText("������ ����Ǿ� ���� �ʽ��ϴ�. �翬�� ��... ��� �� �ٽ� �õ����ּ���.");
            ui.SetUIState(LobbyUIState.Loading);

            ConnectToMasterServer();
        }
    }
    /// <summary>
    /// �⺻ ������ ���� �õ� �� ���� ��� �� ����
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

    // �� ���� �ݹ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ui.SetStatusText("�� ���忡 �����߽��ϴ�. �ٽ� �õ����ּ���.");
        ui.SetUIState(LobbyUIState.Ready);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ui.SetStatusText("�� ������ �����߽��ϴ�. �ٽ� �õ����ּ���.");
        ui.SetUIState(LobbyUIState.Ready);
    }
    public override void OnJoinedRoom()
    {
        ui.SetStatusText("�뿡 ���������� �����߽��ϴ�.");
        ui.SetUIState(LobbyUIState.JoinedRoom);

        SceneManager.LoadScene("MainGameScene");
    }
}