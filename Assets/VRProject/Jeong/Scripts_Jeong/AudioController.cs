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

            // 마스터가 바뀔 경우 사용할 변수를 룸 속성에 저장
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
            float timeSinceStart = (float)PhotonNetwork.Time - startTime; // 경과 시간
            float loopedTime = timeSinceStart % musicSource.clip.length;
            musicSource.time = loopedTime;
            musicSource.Play();
        }
    }
}
