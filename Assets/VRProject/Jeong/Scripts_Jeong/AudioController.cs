using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class AudioController : MonoBehaviourPunCallbacks
{
    public AudioSource musicSource;

    private float musicStartTime;

    private void Awake()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            musicStartTime = (float)PhotonNetwork.Time;
            photonView.RPC("PlayAudio", RpcTarget.All, musicStartTime);

            // �����Ͱ� �ٲ� ��� ����� ������ �� �Ӽ��� ����
            Hashtable props = new Hashtable { { "MusicStartTime", musicStartTime } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayAudio", newPlayer, musicStartTime);
        }
    }
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            musicStartTime = (float)PhotonNetwork.CurrentRoom.CustomProperties["MusicStartTime"];
        }
    }

    [PunRPC]
    public void PlayAudio(float startTime)
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            float timeSinceStart = (float)PhotonNetwork.Time - startTime; // ��� �ð�
            float loopedTime = timeSinceStart % musicSource.clip.length;
            musicSource.time = loopedTime;
            musicSource.Play();
        }
    }
}
