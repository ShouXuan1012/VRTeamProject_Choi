using UnityEngine;
using UnityEngine.UI;

public class RemoteSpeakerUIController : MonoBehaviour
{
    [SerializeField] private Button muteButton;
    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private RemoteSpeakerManager remoteSpeakerManager;

    private void Awake()
    {
        remoteSpeakerManager = transform.parent.GetComponentInChildren<RemoteSpeakerManager>();
    }
    private void Start()
    {
        muteButton.onClick.AddListener(ToggleMute);
        soundIcon.sprite = soundOnSprite;
        soundIcon.color = Color.green;

        UpdateMicStatusUI();
    }
    private void Update()
    {
        UpdateMicStatusUI();
    }

    private void ToggleMute()
    {
        remoteSpeakerManager.ToggleMute();
        soundIcon.sprite = remoteSpeakerManager.IsMuted() ? soundOffSprite : soundOnSprite;
    }
    private void UpdateMicStatusUI()
    {
        //micIcon.sprite = remoteSpeakerManager.IsMuted() ? micOffSprite : micOnSprite;
        soundIcon.color = remoteSpeakerManager.IsSpeaking() ? Color.green : Color.white;
    }
}
