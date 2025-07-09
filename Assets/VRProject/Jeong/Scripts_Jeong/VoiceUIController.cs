using UnityEngine;
using UnityEngine.UI;

public class VoiceUIController : MonoBehaviour
{
    [SerializeField] private Button muteButton;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Sprite micOnSprite;
    [SerializeField] private Sprite micOffSprite;

    [SerializeField] private Text micStatusText;

    private void Start()
    {
        muteButton.onClick.AddListener(ToggleMute);
        UpdateMicStatusText();
    }

    private void ToggleMute()
    {
        VoiceManager.instance.ToggleMute();
        bool isMuted = VoiceManager.instance.IsMuted();
        muteIcon.sprite = isMuted ? micOffSprite : micOnSprite;
    }
    private void UpdateMicStatusText()
    {
        if (!VoiceManager.instance.IsMicrophoneAvailable())
        {
            micStatusText.text = "����ũ ����";
            micStatusText.color = Color.red;
        }
        else
        {
            micStatusText.text = "����ũ ��� ����";
            micStatusText.color = Color.green;
        }
    }
}