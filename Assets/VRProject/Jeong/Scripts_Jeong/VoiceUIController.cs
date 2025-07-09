using UnityEngine;
using UnityEngine.UI;

public class VoiceUIController : MonoBehaviour
{
    [SerializeField] private Button muteButton;
    [SerializeField] private Image muteIcon;
    [SerializeField] private Sprite micOnSprite;
    [SerializeField] private Sprite micOffSprite;

    private void Start()
    {
        muteButton.onClick.AddListener(ToggleMute);
    }

    private void ToggleMute()
    {
        VoiceManager.instance.ToggleMute();
        bool isMuted = VoiceManager.instance.IsMuted();
        muteIcon.sprite = isMuted ? micOffSprite : micOnSprite;
    }
}