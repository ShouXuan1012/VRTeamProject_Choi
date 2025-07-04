using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 접속 버튼
    [SerializeField] private Button connectButton;

    // 연결 상태를 표시할 UI
    [SerializeField] private Text loadingText;
    [SerializeField] private Image loadingImage;

    // 포톤 서버 설정
    private string gameVersion = "1";
    private string region = "kr";

    private void Start()
    {
        SetUIState(false);
    }
    public void Connect()
    {
        SetUIState(true);

        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("마스터 서버에 이미 연결되어 있습니다.");
            loadingText.text = "마스터 서버에 이미 연결되어 있습니다.";

            JoinOrCreateRoom();
        }
        else
        {
            Debug.Log("마스터 서버에 연결 중...");
            loadingText.text = "마스터 서버에 연결 중...";

            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"마스터 서버 연결이 끊어졌습니다: {cause}. 재연결 시도 중...");
        loadingText.text = "마스터 서버 연결이 끊어졌습니다. 재연결 시도 중...";

        Connect();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결되었습니다. 룸 입장 시도 중...");
        loadingText.text = "마스터 서버에 연결되었습니다. 룸 입장 시도 중...";

        JoinOrCreateRoom();
    }
    private void JoinOrCreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("룸 입장 시도 중...");
            loadingText.text = "룸 입장 시도 중...";

            PhotonNetwork.JoinOrCreateRoom(
                "DefaultRoom",
                new RoomOptions { MaxPlayers = 20 },
                TypedLobby.Default
            );
        }
        else
        {
            Debug.Log("네트워크가 준비되지 않았습니다. 다시 시도해주세요.");
            loadingText.text = "네트워크가 준비되지 않았습니다. 다시 시도해주세요.";
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 입장 실패: {message} (코드: {returnCode})");
        loadingText.text = "룸 입장에 실패했습니다. 다시 시도해주세요.";
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"룸 생성 실패: {message} (코드: {returnCode})");
        loadingText.text = "룸 생성에 실패했습니다. 다시 시도해주세요.";
    }
    public override void OnJoinedRoom()
    {
        // 룸 입장 성공
        Debug.Log("룸에 성공적으로 입장했습니다.");
        loadingText.text = "룸에 성공적으로 입장했습니다.";

        // 메인 게임 씬으로 전환
        SceneManager.LoadScene("MainGameScene");
    }
    /// <summary>
    /// 연결 상태에 따라 UI 설정
    /// </summary>
    /// <param name="isConnecting"></param>
    private void SetUIState(bool isConnecting)
    {
        if(isConnecting)
        {
            connectButton.interactable = false;
            loadingText.gameObject.SetActive(true);
            loadingImage.gameObject.SetActive(true);
        }
        else
        {
            connectButton.interactable = true;
            loadingText.gameObject.SetActive(false);
            loadingImage.gameObject.SetActive(false);
        }
    }
}