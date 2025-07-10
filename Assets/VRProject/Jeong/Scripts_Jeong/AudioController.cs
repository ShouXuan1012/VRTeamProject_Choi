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
        Debug.Log("�� ����");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("��������");
            musicStartTime = (float)PhotonNetwork.Time;
            photonView.RPC("PlayAudio", RpcTarget.All, musicStartTime);

            // �����Ͱ� �ٲ� ��� ����� ������ �� �Ӽ��� ����
            Hashtable props = new Hashtable { { "MusicStartTime", musicStartTime } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        else
        {
            Debug.Log("������ �ƴ�");
            // �ٸ� �÷��̾�� �����Ͱ� ������ ���� ���� �ð��� ������
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MusicStartTime", out object startTime))
            {
                musicStartTime = (float)startTime;
                photonView.RPC("PlayAudio", PhotonNetwork.LocalPlayer, musicStartTime);
            }
            else
            {
                Debug.LogWarning("���� ���� �ð��� �������� �ʾҽ��ϴ�.");
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
        Debug.Log("PlayAudio RPC ȣ���, ���� �ð�: " + startTime);
        if (musicSource != null && !musicSource.isPlaying)
        {
            float timeSinceStart = (float)PhotonNetwork.Time - startTime; // ��� �ð�
            float loopedTime = timeSinceStart % musicSource.clip.length;
            musicSource.time = loopedTime;
            musicSource.Play();
        }
    }
}
