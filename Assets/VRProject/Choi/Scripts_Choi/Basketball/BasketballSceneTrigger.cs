using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasketballSceneTrigger : MonoBehaviourPunCallbacks
{
    private bool hasRequested = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasRequested) return;
        if (!other.CompareTag("Player")) return;

        PhotonView view = other.GetComponent<PhotonView>();
        if (view != null && view.IsMine)
        {
            hasRequested = true;
            VoiceManager.instance.Disconnect();
            PhotonNetwork.LeaveRoom(); // ·ë ³ª°¡±â
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room. Waiting for Master Server...");
        StartCoroutine(WaitForMasterThenCreateRoom());
    }

    private IEnumerator WaitForMasterThenCreateRoom()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        string roomName = "Basketball_" + PhotonNetwork.LocalPlayer.NickName + "_" + Random.Range(1000, 9999);
        PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = 1 });
    }

    public override void OnCreatedRoom()
    {
        // ·ë »ý¼ºµÇ¸é ³ó±¸Àå ¾ÀÀ¸·Î ÀÌµ¿
        SceneManager.LoadScene("BasketballScene");
    }
}
