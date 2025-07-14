using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class BasketballEndUI : MonoBehaviourPunCallbacks
{
    public void OnRestartClicked()
    {
        // 농구장 씬 다시 로딩
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitClicked()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined) // In Room 상태일 때만
        {
            StartCoroutine(LeaveRoomAndReturnToMain());
        }
        else
        {
            JoinOrCreateMainRoom(); // 이미 Master 서버에 있으면 바로 Join
        }
    }

    private IEnumerator LeaveRoomAndReturnToMain()
    {
        PhotonNetwork.LeaveRoom();

        // 마스터 서버로 돌아올 때까지 대기
        while (PhotonNetwork.InRoom || PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
        {
            yield return null;
        }

        JoinOrCreateMainRoom();
    }

    private void JoinOrCreateMainRoom()
    {
        string roomName = "DefaultRoom";  // 메인 멀티 방 이름
        RoomOptions options = new RoomOptions { MaxPlayers = 20, IsVisible = true, IsOpen = true };

        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
        }
        else
        {
            Debug.LogWarning("Photon is not connected and ready.");
        }
    }


    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("DevScene_Choi_Test");  // 메인 씬으로 전환
    }
}
