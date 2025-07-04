using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // ���� ��ư
    [SerializeField] private Button connectButton;

    // ���� ���¸� ǥ���� UI
    [SerializeField] private Text loadingText;
    [SerializeField] private Image loadingImage;

    // ���� ���� ����
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
            Debug.Log("������ ������ �̹� ����Ǿ� �ֽ��ϴ�.");
            loadingText.text = "������ ������ �̹� ����Ǿ� �ֽ��ϴ�.";

            JoinOrCreateRoom();
        }
        else
        {
            Debug.Log("������ ������ ���� ��...");
            loadingText.text = "������ ������ ���� ��...";

            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"������ ���� ������ ���������ϴ�: {cause}. �翬�� �õ� ��...");
        loadingText.text = "������ ���� ������ ���������ϴ�. �翬�� �õ� ��...";

        Connect();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ������ ����Ǿ����ϴ�. �� ���� �õ� ��...");
        loadingText.text = "������ ������ ����Ǿ����ϴ�. �� ���� �õ� ��...";

        JoinOrCreateRoom();
    }
    private void JoinOrCreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("�� ���� �õ� ��...");
            loadingText.text = "�� ���� �õ� ��...";

            PhotonNetwork.JoinOrCreateRoom(
                "DefaultRoom",
                new RoomOptions { MaxPlayers = 20 },
                TypedLobby.Default
            );
        }
        else
        {
            Debug.Log("��Ʈ��ũ�� �غ���� �ʾҽ��ϴ�. �ٽ� �õ����ּ���.");
            loadingText.text = "��Ʈ��ũ�� �غ���� �ʾҽ��ϴ�. �ٽ� �õ����ּ���.";
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���� ����: {message} (�ڵ�: {returnCode})");
        loadingText.text = "�� ���忡 �����߽��ϴ�. �ٽ� �õ����ּ���.";
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"�� ���� ����: {message} (�ڵ�: {returnCode})");
        loadingText.text = "�� ������ �����߽��ϴ�. �ٽ� �õ����ּ���.";
    }
    public override void OnJoinedRoom()
    {
        // �� ���� ����
        Debug.Log("�뿡 ���������� �����߽��ϴ�.");
        loadingText.text = "�뿡 ���������� �����߽��ϴ�.";

        // ���� ���� ������ ��ȯ
        SceneManager.LoadScene("MainGameScene");
    }
    /// <summary>
    /// ���� ���¿� ���� UI ����
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