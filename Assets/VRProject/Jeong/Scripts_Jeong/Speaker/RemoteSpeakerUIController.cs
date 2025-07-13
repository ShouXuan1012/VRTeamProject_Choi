using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RemoteSpeakerUIController : MonoBehaviourPun
{
    [SerializeField] private GameObject remoteVoiceCanvas;

    [SerializeField] private Button muteButton;
    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private RemoteSpeakerManager remoteSpeakerManager;

    private void Start()
    {
        if (photonView.IsMine)
        {
            remoteVoiceCanvas.gameObject.SetActive(false);
            return;
        }

        remoteSpeakerManager = transform.root.GetComponentInChildren<RemoteSpeakerManager>();

        muteButton.onClick.AddListener(ToggleMute);
        soundIcon.sprite = soundOnSprite;
        soundIcon.color = Color.green;

        UpdateMicStatusUI();
    }
    private void Update()
    {
        if (photonView.IsMine) return;

        UpdateMicStatusUI();
    }

    private void ToggleMute()
    {
        remoteSpeakerManager.ToggleMute();
        soundIcon.sprite = remoteSpeakerManager.IsMuted() ? soundOffSprite : soundOnSprite;
    }
    private void UpdateMicStatusUI()
    {
        soundIcon.color = remoteSpeakerManager.IsSpeaking() ? Color.green : Color.white;
    }
}
