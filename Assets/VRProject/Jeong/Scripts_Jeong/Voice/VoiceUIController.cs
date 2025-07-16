using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class VoiceUIController : MonoBehaviourPun
{
    [SerializeField] private GameObject localVoiceCanvas;

    [SerializeField] private Button muteButton;
    [SerializeField] private Image micIcon;
    [SerializeField] private Sprite micOnSprite;
    [SerializeField] private Sprite micOffSprite;

    [SerializeField] private Text micStatusText;

    private void Start()
    {
        if (!photonView.IsMine)
        {
            localVoiceCanvas.SetActive(false);
            return;
        }

        localVoiceCanvas.SetActive(true);

        muteButton.onClick.AddListener(ToggleMute);
        micIcon.sprite = micOffSprite;
        micIcon.color = Color.red;

        UpdateMicStatusUI();
    }
    private void Update()
    {
        if (!photonView.IsMine) return;

        UpdateMicStatusUI();
    }

    private void ToggleMute()
    {
        if (VoiceManager.instance.IsMicrophoneAvailable())
        {
            VoiceManager.instance.ToggleMute();
            micIcon.sprite = VoiceManager.instance.IsMuted() ? micOffSprite : micOnSprite;
        }
    }
    private void UpdateMicStatusUI()
    {
        if (VoiceManager.instance.IsMicrophoneAvailable())
        {
            micStatusText.text = "마이크: " + VoiceManager.instance.GetMicrophoneName();
            micStatusText.color = Color.green;

            micIcon.color = VoiceManager.instance.IsSpeaking() ? Color.green : Color.white;
        }
        else
        {
            micStatusText.text = "마이크 없음";
            micStatusText.color = Color.red;

            micIcon.sprite = micOffSprite;
            micIcon.color = Color.red;
        }
    }
}