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
        Debug.Log("방 들어옴");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터임");
            musicStartTime = (float)PhotonNetwork.Time;
            photonView.RPC("PlayAudio", RpcTarget.All, musicStartTime);

            // 마스터가 바뀔 경우 사용할 변수를 룸 속성에 저장
            Hashtable props = new Hashtable { { "MusicStartTime", musicStartTime } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        else
        {
            Debug.Log("마스터 아님");
            // 다른 플레이어는 마스터가 설정한 음악 시작 시간을 가져옴
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MusicStartTime", out object startTime))
            {
                musicStartTime = (float)startTime;
                photonView.RPC("PlayAudio", PhotonNetwork.LocalPlayer, musicStartTime);
            }
            else
            {
                Debug.LogWarning("음악 시작 시간이 설정되지 않았습니다.");
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
        Debug.Log("PlayAudio RPC 호출됨, 시작 시간: " + startTime);
        if (musicSource != null && !musicSource.isPlaying)
        {
            float timeSinceStart = (float)PhotonNetwork.Time - startTime; // 경과 시간
            float loopedTime = timeSinceStart % musicSource.clip.length;
            musicSource.time = loopedTime;
            musicSource.Play();
        }
    }
}
