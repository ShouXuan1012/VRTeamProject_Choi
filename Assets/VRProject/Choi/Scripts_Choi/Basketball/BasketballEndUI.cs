using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class BasketballEndUI : MonoBehaviourPunCallbacks
{
    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitClicked()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            StartCoroutine(LeaveRoomAndReturnToMain());
        }
        else
        {
            JoinOrCreateMainRoom();
        }
    }

    private IEnumerator LeaveRoomAndReturnToMain()
    {
        PhotonNetwork.LeaveRoom();

        // 대기
        while (PhotonNetwork.InRoom || PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
        {
            yield return null;
        }

        JoinOrCreateMainRoom();
    }

    private void JoinOrCreateMainRoom()
    {
        string roomName = "DefaultRoom";  // 메인 멀티 방 이름
        RoomOptions options = new RoomOptions { MaxPlayers = 20 };

        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("DevScene_Choi_Test");
    }
}
