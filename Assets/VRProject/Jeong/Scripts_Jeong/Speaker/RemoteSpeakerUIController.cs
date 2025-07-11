using UnityEngine;
using UnityEngine.UI;

public class RemoteSpeakerUIController : MonoBehaviour
{
    [SerializeField] private Button muteButton;
    [SerializeField] private Image micIcon;
    [SerializeField] private Sprite micOnSprite;
    [SerializeField] private Sprite micOffSprite;

    private RemoteSpeakerManager remoteSpeakerManager;

    private void Awake()
    {
        remoteSpeakerManager = transform.parent.GetComponentInChildren<RemoteSpeakerManager>();
    }
    private void Start()
    {
        muteButton.onClick.AddListener(ToggleMute);
        micIcon.sprite = micOnSprite;
        micIcon.color = Color.green;

        UpdateMicStatusUI();
    }
    private void Update()
    {
        UpdateMicStatusUI();
    }

    private void ToggleMute()
    {
        remoteSpeakerManager.ToggleMute();
        micIcon.sprite = remoteSpeakerManager.IsMuted() ? micOffSprite : micOnSprite;
    }
    private void UpdateMicStatusUI()
    {
        //micIcon.sprite = remoteSpeakerManager.IsMuted() ? micOffSprite : micOnSprite;
        micIcon.color = remoteSpeakerManager.IsSpeaking() ? Color.green : Color.white;
    }
}
