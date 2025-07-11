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
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            musicStartTime = (float)PhotonNetwork.Time;
            photonView.RPC("PlayAudio", RpcTarget.All, musicStartTime);

            Hashtable props = new Hashtable { { "MusicStartTime", musicStartTime } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MusicStartTime", out object startTime))
            {
                musicStartTime = (float)startTime;
                photonView.RPC("PlayAudio", PhotonNetwork.LocalPlayer, musicStartTime);
            }
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
